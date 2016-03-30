namespace BadExample.Service.Interfaces
{
    public interface IAmazonWebService
    {
        void UploadFileToBucket(string localFilePath, string amazonFilePath, string bucketName);
    }
}