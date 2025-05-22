namespace LicenseDelivery.API.LicenseDelivery.GetLicense
{
    public class GoogleDriveClient : IGoogleDriveClient
    {
        private static DriveService _service;
        private readonly string _googleFolderId;
        public GoogleDriveClient(
            DriveService service,
            GoogleDriveOptions options
        )
        {
            _service = service;
            _googleFolderId = options.FolderId;
        }

        public async Task DeleteFile(string fileId)
        {
            var request = _service.Files.Delete(fileId);
            string response = await request.ExecuteAsync();
            if (response == null)
            {
                throw new InvalidOperationException(
                    $"Failed to delete file with id '{fileId}'. " +
                    "Google Drive API returned no confirmation.");
            }
        }

        public async Task<string> GetFileByName(string fileName)
        {
            var fileList = _service.Files.List();

            fileList.Q = $"name='{fileName}' and '{_googleFolderId}' in parents";
            fileList.Fields = "nextPageToken,files(id,name,size,mimeType,createdTime)";
            var result = new List<Google.Apis.Drive.v3.Data.File>();

            string pageToken = null;

            do
            {
                fileList.PageToken = pageToken;
                var fileResult = await fileList.ExecuteAsync();
                var files = fileResult.Files;
                pageToken = fileResult.NextPageToken;
                result.AddRange(files);
            } while (pageToken != null);

            var file = result.FirstOrDefault();
            if (file is null)
                throw new GoogleDriveFileNotFoundException(fileName);

            return file.Id;   
        }
    }
}
