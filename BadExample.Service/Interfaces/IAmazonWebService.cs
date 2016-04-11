using BadExample.Service.Models;

namespace BadExample.Service.Interfaces
{
    public interface IAmazonWebService
    {
        ResponseObject<bool> UploadFileToBucket(string localFilePath, string amazonFilePath, string bucketName);
    }
}