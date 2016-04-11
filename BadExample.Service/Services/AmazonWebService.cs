using System;
using System.Configuration;
using System.IO;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using BadExample.Service.Interfaces;
using BadExample.Service.Models;

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
        public ResponseObject<bool> UploadFileToBucket(string localFilePath, string amazonFilePath, string bucketName)
        {
            try
            {
                if (!File.Exists(localFilePath))
                {
                    return new ResponseObject<bool>
                    {
                        Error = new ArgumentException($"{localFilePath}- file does not exist in UploadFileToS3 in AmazonWebService Package"),
                        Value = false
                    };
                }

                var client = new AmazonS3Client(GetCredentials(), Amazon.RegionEndpoint.USEast1);
                var request = new PutObjectRequest
                {
                    FilePath = localFilePath,
                    BucketName = bucketName,
                    Key = amazonFilePath,
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
                };
                client.PutObject(request);

                return new ResponseObject<bool>
                {
                    Error = null,
                    Value = true
                };
            }
            catch (Exception e)
            {
                return new ResponseObject<bool>
                {
                    Error = new ArgumentException($"There was an exception thrown in UploadFileToBucket:{e}"),
                    Value = false
                };
            }
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