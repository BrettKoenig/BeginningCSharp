using System.Configuration;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using BadExample.Service.Interfaces;

namespace BadExample.Service.Services
{
    public class AmazonWebService : IAmazonWebService
    {
        private readonly string _awsAccessKey;
        private readonly string _awsSecretKey;
        public AmazonWebService()
        {
            _awsAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            _awsSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        }
        public void UploadFileToBucket(string localFilePath, string amazonFilePath, string bucketName)
        {
            var amazonClient = new AmazonS3Client(GetCredentials(), Amazon.RegionEndpoint.USEast1);
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = amazonFilePath,
                FilePath = localFilePath
            };
            PutObjectResponse response = amazonClient.PutObject(request);
        }

        private SessionAWSCredentials GetCredentials()
        {
            var stsClient = new AmazonSecurityTokenServiceClient(_awsAccessKey,
                _awsSecretKey);

            var getSessionTokenRequest = new GetSessionTokenRequest
            {
                DurationSeconds = 3600
            };

            var sessionTokenResponse = stsClient.GetSessionToken(getSessionTokenRequest);
            var credentials = sessionTokenResponse.Credentials;

            var sessionCredentials = new SessionAWSCredentials(credentials.AccessKeyId, credentials.SecretAccessKey,
                credentials.SessionToken);

            return sessionCredentials;
        }
    }
}