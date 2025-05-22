namespace LicenseDelivery.API.LicenseDelivery.Infrastructure.GoogleDrive
{
    public interface IGoogleDriveClient
    {
        Task DeleteFile(string fileId);
        Task<string> GetFileByName(string fileName);
    }
}
