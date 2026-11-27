namespace Amsberg_digital_universe.Domain.Common.Storage;

public class StorageFile
{
    public Guid Id { get; private set; }
    public string Container { get; private set; }
    public string Key { get; private set; }
    public Guid AuthorId { get; private set; }
    public Profile.Profile Author { get; private set; } = null!;
    public StorageFileStatus Status { get; private set; } = StorageFileStatus.Pending;
    public DateTime CreatedAt { get; private set; }

    public static StorageFile Create(string container, string key, Guid authorId, DateTime now) => new()
    {
        Id = Guid.CreateVersion7(),
        Container = container,
        Key = key,
        AuthorId = authorId,
        CreatedAt = now
    };

    public void ConfirmUpload()
    {
        if(Status is StorageFileStatus.Pending)
            Status = StorageFileStatus.Confirmed;
    }

    public void Reject()
    {
        if (Status is not StorageFileStatus.Confirmed)
            Status = StorageFileStatus.Rejected;
    }

    public void Delete()
    {
        if (Status is not StorageFileStatus.Deleted)
            Status = StorageFileStatus.Deleted;
    }
}