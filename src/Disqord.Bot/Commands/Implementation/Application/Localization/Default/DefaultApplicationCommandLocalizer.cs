using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Serialization.Json;
using Disqord.Serialization.Json.Default;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Collections;
using Qommon.Pooling;

namespace Disqord.Bot.Commands.Application.Default;

public partial class DefaultApplicationCommandLocalizer : IApplicationCommandLocalizer, ILogging
{
    public ILogger Logger { get; }

    public IJsonSerializer Serializer { get; }

    public string DirectoryPath { get; }

    public string LocaleFileNameFormat { get; }

    public string TemporaryFileNameFormat { get; }

    public string BackupFileNameFormat { get; }

    public CultureInfo DefaultCulture { get; }

    public const int SchemaVersion = 1;

    public DefaultApplicationCommandLocalizer(
        IOptions<DefaultApplicationCommandLocalizerConfiguration> options,
        ILogger<DefaultApplicationCommandLocalizer> logger,
        IJsonSerializer serializer)
    {
        var configuration = options.Value;
        DirectoryPath = Path.GetFullPath(configuration.DirectoryPath);
        LocaleFileNameFormat = configuration.LocaleFileNameFormat;
        TemporaryFileNameFormat = configuration.TemporaryFileNameFormat;
        BackupFileNameFormat = configuration.BackupFileNameFormat;
        DefaultCulture = configuration.DefaultCulture;

        Logger = logger;
        Serializer = serializer;
    }

    protected virtual StoreInformation CreateStoreInformation(CultureInfo locale, string localeFilePath, bool localeExists, MemoryStream memoryStream, LocalizationStoreJsonModel model)
    {
        var store = new StoreInformation(this, locale, localeFilePath, localeExists, memoryStream, model);
        store.Migrate();
        return store;
    }

    public async ValueTask LocalizeAsync(IEnumerable<LocalApplicationCommand> globalCommands,
        IReadOnlyDictionary<Snowflake, IEnumerable<LocalApplicationCommand>> guildCommands, CancellationToken cancellationToken = default)
    {
        var directoryPath = DirectoryPath;
        try
        {
            // Creates the directory for localizations.
            Directory.CreateDirectory(directoryPath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create the directory for localizations '{directoryPath}'.", ex);
        }

        var fileWildcard = string.Format(LocaleFileNameFormat, "*");
        List<(string LocaleName, string LocaleFilePath)> existingLocales;
        var localeNames = Discord.LocaleNames.GetArray();
        try
        {
            // Finds existing localization files.
            existingLocales = new List<(string, string)>();
            foreach (var existingLocaleFilePath in Directory.EnumerateFiles(directoryPath, fileWildcard))
            {
                var localeName = Path.GetFileNameWithoutExtension(existingLocaleFilePath);
                if (Array.IndexOf(localeNames, localeName) == -1)
                    continue;

                existingLocales.Add((localeName, existingLocaleFilePath));
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get the existing localizations from the directory '{directoryPath}'.", ex);
        }

        var stores = new List<StoreInformation>(localeNames.Length);
        var missingLocaleNames = localeNames.Except(existingLocales.Select(existingLocale => existingLocale.LocaleName)).ToArray();
        if (missingLocaleNames.Length > 0)
            Logger.LogDebug("Missing localization resource files that will be created: {0}", missingLocaleNames as object);

        var existingLocaleFilePathCount = existingLocales.Count;
        Task<StoreInformation[]> resultsTask;
        using (var tasks = RentedArray<Task>.Rent(existingLocaleFilePathCount))
        {
            for (var i = 0; i < existingLocaleFilePathCount; i++)
            {
                var existingLocale = existingLocales[i];
                tasks[i] = Task.Factory.StartNew(static async state =>
                {
                    var (@this, existingLocale, cancellationToken) = ((DefaultApplicationCommandLocalizer, (string LocaleName, string LocaleFilePath), CancellationToken)) state!;

                    try
                    {
                        var memoryStream = new MemoryStream();
                        var fileStream = new FileStream(existingLocale.LocaleFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

                        try
                        {
                            await fileStream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
                        }
                        finally
                        {
                            await fileStream.DisposeAsync().ConfigureAwait(false);
                        }

                        memoryStream.Position = 0;

                        LocalizationStoreJsonModel? model;
                        try
                        {
                            model = @this.Serializer.Deserialize<LocalizationStoreJsonModel>(memoryStream);
                        }
                        catch (JsonSerializationException)
                        {
                            model = null;
                        }

                        memoryStream.Position = 0;

                        return @this.CreateStoreInformation(CultureInfo.GetCultureInfo(existingLocale.LocaleName), existingLocale.LocaleFilePath, model != null && model.SchemaVersion != 0, memoryStream, model ?? new());
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Failed to read the localization file '{existingLocale.LocaleFilePath}'.", ex);
                    }
                }, (this, existingLocale, cancellationToken), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
            }

            resultsTask = Task.WhenAll(tasks.Cast<Task<StoreInformation>>());
        }

        var continuation = resultsTask.ContinueWith(_ => { }, default(CancellationToken));
        await continuation.ConfigureAwait(false);
        if (resultsTask.Exception != null)
        {
            throw resultsTask.Exception!;
        }

        foreach (var storeInformation in resultsTask.Result)
        {
            stores.Add(storeInformation);
        }

        foreach (var missingLocaleName in missingLocaleNames)
        {
            stores.Add(CreateStoreInformation(CultureInfo.GetCultureInfo(missingLocaleName), Path.Join(directoryPath, string.Format(LocaleFileNameFormat, missingLocaleName)), false, new(), new()));
        }

        foreach (var storeInformation in stores)
        {
            storeInformation.Execute(null, globalCommands);

            foreach (var kvp in guildCommands)
                storeInformation.Execute(kvp.Key, kvp.Value);
        }

        using (var tasks = RentedArray<Task>.Rent(stores.Count))
        {
            var index = 0;
            foreach (var storeInformation in stores)
            {
                tasks[index++] = Task.Factory.StartNew(static async state =>
                {
                    var (@this, storeInformation, cancellationToken) = ((DefaultApplicationCommandLocalizer, StoreInformation, CancellationToken)) state!;
                    var createdTemporaryFile = false;
                    var temporaryFileName = string.Format(@this.TemporaryFileNameFormat, storeInformation.Locale.Name);
                    var temporaryFilePath = Path.Join(@this.DirectoryPath, temporaryFileName);
                    var memoryStream = storeInformation.MemoryStream;
                    @this.Serializer.Serialize(memoryStream, storeInformation.Model, new DefaultJsonSerializerOptions
                    {
                        Formatting = JsonFormatting.Indented
                    });

                    memoryStream.SetLength(memoryStream.Position);
                    memoryStream.Position = 0;

                    var localeFileExists = storeInformation.LocaleFileExists;
                    var localeFilePath = storeInformation.LocaleFilePath;
                    try
                    {
                        if (localeFileExists)
                        {
                            FileStream fileStream;
                            try
                            {
                                fileStream = new FileStream(temporaryFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                                createdTemporaryFile = true;
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidOperationException($"Failed to create the temporary file '{temporaryFilePath}'", ex);
                            }

                            try
                            {
                                await memoryStream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
                                await fileStream.FlushAsync(cancellationToken).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidOperationException($"Failed to write to the temporary file '{temporaryFilePath}'.", ex);
                            }
                            finally
                            {
                                await fileStream.DisposeAsync().ConfigureAwait(false);
                            }

                            var backupFilePath = Path.Join(@this.DirectoryPath, string.Format(@this.BackupFileNameFormat, storeInformation.Locale.Name));
                            for (var i = 0; i < 5; i++)
                            {
                                if (i > 0)
                                    await Task.Delay(i, cancellationToken).ConfigureAwait(false);

                                try
                                {
                                    File.Replace(temporaryFilePath, localeFilePath, backupFilePath);
                                    createdTemporaryFile = false;
                                    try
                                    {
                                        File.Delete(backupFilePath);
                                    }
                                    catch { }

                                    break;
                                }
                                catch (IOException ex) when ((ex.HResult & ~0x80070000) == 0x497) // ERROR_UNABLE_TO_REMOVE_REPLACED
                                {
#if DEBUG
                                    Console.WriteLine($"Locale exists: {File.Exists(localeFilePath)}, temporary locale exists: {File.Exists(temporaryFilePath)}\nException:{ex}");
#endif
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    throw new InvalidOperationException($"An exception occurred while replacing the localization file '{localeFilePath}' with '{temporaryFilePath}'.", ex);
                                }
                            }
                        }
                        else
                        {
                            FileStream fileStream;
                            try
                            {
                                fileStream = new FileStream(localeFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidOperationException($"Failed to create the missing localization file '{localeFilePath}'.", ex);
                            }

                            try
                            {
                                await memoryStream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
                                await fileStream.FlushAsync(cancellationToken).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidOperationException($"Failed to write to the temporary file '{temporaryFilePath}'.", ex);
                            }
                            finally
                            {
                                await fileStream.DisposeAsync().ConfigureAwait(false);
                            }
                        }
                    }
                    finally
                    {
                        if (createdTemporaryFile)
                        {
                            try
                            {
                                File.Delete(temporaryFilePath);
                            }
                            catch (Exception ex)
                            {
                                @this.Logger.LogWarning(ex, "An exception occurred while deleting the temporary file '{0}'.", temporaryFilePath);
                            }
                        }
                    }
                }, (this, storeInformation, cancellationToken), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
