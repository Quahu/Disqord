using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Bot.Commands.Application.Default;

public partial class DefaultApplicationCommandCacheProvider : IApplicationCommandCacheProvider
{
    public ILogger<DefaultApplicationCommandCacheProvider> Logger { get; }

    public IJsonSerializer Serializer { get; }

    public string DirectoryPath { get; }

    public string FilePath { get; }

    public string TemporaryFilePath { get; }

    public string BackupFilePath { get; }

    public const int SchemaVersion = 1;

    public DefaultApplicationCommandCacheProvider(
        IOptions<DefaultApplicationCommandCacheProviderConfiguration> options,
        ILogger<DefaultApplicationCommandCacheProvider> logger,
        IJsonSerializer serializer)
    {
        var configuration = options.Value;
        DirectoryPath = Path.GetFullPath(configuration.DirectoryPath);
        var directoryPath = DirectoryPath;
        var fileName = configuration.FileName;
        FilePath = Path.Join(directoryPath, string.Format(configuration.FileNameFormat, fileName));
        TemporaryFilePath = Path.Join(directoryPath, string.Format(configuration.TemporaryFileNameFormat, fileName));
        BackupFilePath = Path.Join(directoryPath, string.Format(configuration.BackupFileNameFormat, fileName));
        Logger = logger;
        Serializer = serializer;
    }

    protected virtual Cache CreateCache(bool cacheFileExists, MemoryStream memoryStream, CacheJsonModel model)
    {
        var cache = new Cache(this, cacheFileExists, memoryStream, model);
        cache.Migrate();
        return cache;
    }

    public virtual async ValueTask<IApplicationCommandCache> GetCacheAsync(CancellationToken cancellationToken)
    {
        var directoryPath = DirectoryPath;
        try
        {
            // Creates the directory for the cache.
            Directory.CreateDirectory(directoryPath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create the directory for the cache '{directoryPath}'.", ex);
        }

        var memoryStream = new MemoryStream();
        CacheJsonModel? model;
        try
        {
            var fileStream = new FileStream(FilePath, FileMode.Open);

            try
            {
                await fileStream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                await fileStream.DisposeAsync().ConfigureAwait(false);
            }

            memoryStream.Position = 0;

            try
            {
                model = Serializer.Deserialize<CacheJsonModel>(memoryStream);
            }
            catch (JsonSerializationException)
            {
                model = null;
            }

            memoryStream.Position = 0;
        }
        catch (FileNotFoundException)
        {
            model = null;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An exception occurred while getting the cache.", ex);
        }

        return CreateCache(model != null && model.SchemaVersion != 0, memoryStream, model ?? new());
    }

    protected virtual async ValueTask DisposeCacheAsync(Cache cache)
    {
        if (!cache.HasChanges)
            return;

        var memoryStream = cache.MemoryStream;
        Serializer.Serialize(memoryStream, cache.Model);

        memoryStream.SetLength(memoryStream.Position);
        memoryStream.Position = 0;

        if (cache.CacheFileExists)
        {
            var createdTemporaryFile = false;
            try
            {
                FileStream fileStream;
                try
                {
                    fileStream = new FileStream(TemporaryFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    createdTemporaryFile = true;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create the temporary file '{TemporaryFilePath}'", ex);
                }

                try
                {
                    await cache.MemoryStream.CopyToAsync(fileStream).ConfigureAwait(false);
                    await fileStream.FlushAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to write to the temporary file '{TemporaryFilePath}'.", ex);
                }
                finally
                {
                    await fileStream.DisposeAsync().ConfigureAwait(false);
                }

                for (var i = 0; i < 5; i++)
                {
                    if (i > 0)
                        await Task.Delay(i).ConfigureAwait(false);

                    try
                    {
                        var backupFilePath = BackupFilePath;
                        File.Replace(TemporaryFilePath, FilePath, backupFilePath);
                        createdTemporaryFile = false;
                        try
                        {
                            File.Delete(backupFilePath);
                        }
                        catch { }

                        break;
                    }
                    catch (IOException ex) when ((ex.HResult & ~0x80070000) == 0x497) // ERROR_UNABLE_TO_REMOVE_REPLACED
                    { }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"An exception occurred while replacing the cache file '{FilePath}' with '{TemporaryFilePath}'.", ex);
                    }
                }
            }
            finally
            {
                if (createdTemporaryFile)
                {
                    try
                    {
                        File.Delete(TemporaryFilePath);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex, "An exception occurred while deleting the temporary file '{0}'.", TemporaryFilePath);
                    }
                }
            }
        }
        else
        {
            FileStream fileStream;
            try
            {
                fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create the cache file '{FilePath}'.", ex);
            }

            try
            {
                await cache.MemoryStream.CopyToAsync(fileStream).ConfigureAwait(false);
                await fileStream.FlushAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to write to the temporary file '{TemporaryFilePath}'.", ex);
            }
            finally
            {
                await fileStream.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
