namespace Amsberg_digital_universe.Domain.Common.Storage;

public class StorageFile
{
    public Guid Id { get; private set; }
    public string Container { get; private set; }
    public string? Key { get; private set; }
    public Guid AuthorId { get; private set; }
    public Profile.Profile Author { get; private set; } = null!;
    public StorageFileStatus Status { get; private set; } = StorageFileStatus.Pending;

    public static StorageFile Create(string container, string key, Guid authorId) => new()
    {
        Id = Guid.CreateVersion7(),
        Container = container,
        Key = key,
        AuthorId = authorId
    };

    public void ConfirmUpdate()
    {
        Status = StorageFileStatus.Confirmed;
    }
}