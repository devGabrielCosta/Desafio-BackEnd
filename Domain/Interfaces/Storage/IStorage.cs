namespace Domain.Interfaces.Storage
{
    public interface IStorage
    {
        Task<string> UploadFile(Stream fileStream, string keyName);
    }
}
