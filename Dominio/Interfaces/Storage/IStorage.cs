namespace Dominio.Interfaces.Storage
{
    public interface IStorage
    {
        Task<string> UploadFile(Stream fileStream, string keyName);
    }
}
