namespace LicenseDelivery.API.Exceptions
{
    public class GoogleDriveFileNotFoundException : NotFoundException
    {
        public GoogleDriveFileNotFoundException(string fileId)
            : base($"File with '{fileId}' was not found in Google Drive.")
        {
        }
    }
}
