namespace BadExample.Service
{
    public interface IAmazonWebService
    {
        void UploadFileToBucket(string localFilePath, string amazonFilePath, string bucketName);
    }
}