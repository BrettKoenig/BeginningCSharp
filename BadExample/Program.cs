using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using S22.Imap;

namespace BadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Attachments from Gmail
            ImapClient client = new ImapClient("imap.gmail.com", 993, true);
            client.Login(ConfigurationManager.AppSettings["EmailUsername"], ConfigurationManager.AppSettings["EmailPassword"], AuthMethod.Auto);
            var emailsFromSender = client.Search(SearchCondition.From(ConfigurationManager.AppSettings["SearchFromEmail"]));
            foreach (var email in emailsFromSender)
            {
                var attachmentsOnEmail = client.GetMessage(email).Attachments;
                foreach (var attachment in attachmentsOnEmail)
                {
                    var localFile = File.Create(ConfigurationManager.AppSettings["TempFileLocation"] + attachment.Name);
                    attachment.ContentStream.Seek(0, SeekOrigin.Begin);
                    attachment.ContentStream.CopyTo(localFile);
                    localFile.Close();
                }
            }
            //Read Info from files
            int counter = 0;
            string line = string.Empty;
            foreach (var attachmentFile in Directory.GetFiles(ConfigurationManager.AppSettings["TempFileLocation"]))
            {
                StreamReader fileStream = new StreamReader(attachmentFile);
                while ((line = fileStream.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    var lineSplit = line.Split(',');
                    if (lineSplit.Length != 5)
                    {
                        break;
                    }
                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        var returnVal = 0;
                        var command = new SqlCommand("INSERT INTO Candy_Inventory (ID, Name, Type, Amount, Cost) values (@id, @name, @type, @amount, @cost)");
                        command.Parameters.AddWithValue("id", lineSplit[0]);
                        command.Parameters.AddWithValue("name", lineSplit[1]);
                        command.Parameters.AddWithValue("type", lineSplit[2]);
                        command.Parameters.AddWithValue("amount", lineSplit[3]);
                        command.Parameters.AddWithValue("cost", Convert.ToDouble(lineSplit[4]));

                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            returnVal = Convert.ToInt32(reader[0]);
                            Console.WriteLine(returnVal);
                        }
                        reader.Close();
                    }
                    counter++;
                }
                fileStream.Close();
                counter = 0;
            }
            //Store files on s3
            foreach (var file in Directory.GetFiles(ConfigurationManager.AppSettings["TempFileLocation"]))
            {
                var amazonClient = new AmazonS3Client(GetCredentials(), Amazon.RegionEndpoint.USEast1);
                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = "Attachments Bucket",
                    Key = file,
                    FilePath = file
                };
                PutObjectResponse response = amazonClient.PutObject(request);
            }
            //Delete files locally
            foreach (var localFileToDelete in Directory.GetFiles(ConfigurationManager.AppSettings["TempFileLocation"]))
            {
                File.Delete(localFileToDelete);
            }
        }
        private static SessionAWSCredentials GetCredentials()
        {
            var stsClient = new AmazonSecurityTokenServiceClient(ConfigurationManager.AppSettings["AWSAccessKey"], ConfigurationManager.AppSettings["AWSSecretKey"]);

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