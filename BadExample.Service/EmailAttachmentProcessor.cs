using System;
using System.Data.SqlClient;
using System.IO;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using S22.Imap;

namespace BadExample.Service
{
    public class EmailAttachmentProcessor
    {
        private readonly string _emailUsername;
        private readonly string _emailPassword;
        private readonly string _searchFromEmail;
        private readonly string _tempFileLocation;
        private readonly string _connectionString;
        private readonly string _awsAccessKey;
        private readonly string _awsSecretKey;
        public EmailAttachmentProcessor(string emailUsername, string emailPassword, string searchFromEmail, string tempFileLocation, string connectionString, string awsAccessKey, string awsSecretKey)
        {
            _emailUsername = emailUsername;
            _emailPassword = emailPassword;
            _searchFromEmail = searchFromEmail;
            _tempFileLocation = tempFileLocation;
            _connectionString = connectionString;
            _awsAccessKey = awsAccessKey;
            _awsSecretKey = awsSecretKey;
        }
        public void ProcessEmailAttachments()
        {
            //Get Attachments from Gmail
            ImapClient client = new ImapClient("imap.gmail.com", 993, true);
            client.Login(_emailUsername, _emailPassword, AuthMethod.Auto);
            var emailsFromSender = client.Search(SearchCondition.From(_searchFromEmail));
            foreach (var email in emailsFromSender)
            {
                var attachmentsOnEmail = client.GetMessage(email).Attachments;
                //nested foreach may not be the best solution these days.. Linq?
                foreach (var attachment in attachmentsOnEmail)
                {
                    var localFile = File.Create(_tempFileLocation + attachment.Name);
                    attachment.ContentStream.Seek(0, SeekOrigin.Begin);
                    attachment.ContentStream.CopyTo(localFile);
                    localFile.Close();

                    //Read Info from files
                    string line = string.Empty;
                    StreamReader fileStream = new StreamReader(localFile);
                    while ((line = fileStream.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        var lineSplit = line.Split(',');
                        if (lineSplit.Length != 5)
                        {
                            break;
                        }
                        using (var connection = new SqlConnection(_connectionString))
                        {
                            var command =
                                new SqlCommand(
                                    "INSERT INTO Candy_Inventory (ID, Name, Type, Amount, Cost) values (@id, @name, @type, @amount, @cost)");
                            command.Parameters.AddWithValue("id", lineSplit[0]);
                            command.Parameters.AddWithValue("name", lineSplit[1]);
                            command.Parameters.AddWithValue("type", lineSplit[2]);
                            command.Parameters.AddWithValue("amount", lineSplit[3]);
                            command.Parameters.AddWithValue("cost", Convert.ToDouble(lineSplit[4]));

                            connection.Open();
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                int returnVal = Convert.ToInt32(reader[0]);
                                Console.WriteLine(returnVal);
                            }
                            reader.Close();
                        }
                    }
                    fileStream.Close();
                    //Store files on s3
                    var amazonClient = new AmazonS3Client(GetCredentials(), Amazon.RegionEndpoint.USEast1);
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        BucketName = "Attachments Bucket",
                        Key = localFile.Name,
                        FilePath = localFile.Name
                    };
                    PutObjectResponse response = amazonClient.PutObject(request);
                    //Delete files locally
                    File.Delete(localFile.Name);
                }
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
