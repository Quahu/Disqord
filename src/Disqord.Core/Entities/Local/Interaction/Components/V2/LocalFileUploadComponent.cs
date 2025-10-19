using Qommon;

namespace Disqord;

public class LocalFileUploadComponent : LocalComponent, ILocalConstruct<LocalFileUploadComponent>
{
    /// <summary>
    ///     Gets or sets the custom ID of this file upload.
    /// </summary>
    public Optional<string> CustomId { get; set; }

    /// <summary>
    ///     Gets or sets the minimum number of files required to be uploaded.
    /// </summary>
    public Optional<int> MinimumUploadedFiles { get; set; }

    /// <summary>
    ///     Gets or sets the maximum number of files allowed to be uploaded.
    /// </summary>
    public Optional<int> MaximumUploadedFiles { get; set; }

    /// <summary>
    ///     Gets or sets whether 
    /// </summary>
    public Optional<bool> IsRequired { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalFileUploadComponent"/>.
    /// </summary>
    public LocalFileUploadComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalLabelComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalFileUploadComponent(LocalFileUploadComponent other)
    {
        CustomId = other.CustomId;
        MinimumUploadedFiles = other.MinimumUploadedFiles;
        MaximumUploadedFiles = other.MaximumUploadedFiles;
        IsRequired = other.IsRequired;
    }

    /// <inheritdoc/>
    public override LocalFileUploadComponent Clone()
    {
        return new(this);
    }
}
