using System;
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
            client.Login("emailToSearch@.com", "secret", AuthMethod.Auto);
            var things = client.Search(SearchCondition.From("senderEmail@.com"));
            foreach (var thing in things)
            {
                var thingFromThings = client.GetMessage(thing).Attachments;
                foreach (var thingFromThing in thingFromThings)
                {
                    var newThing = File.Create("C:\\Users\\bkoenig\\Desktop\\Temp\\" + thingFromThing.Name);
                    thingFromThing.ContentStream.Seek(0, SeekOrigin.Begin);
                    thingFromThing.ContentStream.CopyTo(newThing);
                    newThing.Close();
                }
            }
            //Read Info from files
            int counter = 0;
            string line;
            foreach (var thing in Directory.GetFiles("C:\\Users\\bkoenig\\Desktop\\Temp\\"))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(thing);
                while ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    //you know exactly what this is below, don't use var even though resharper will say so
                    var lineSplit = line.Split(',');
                    //basic error catching we don't think it's a csv... What if the first line is just messed up, or it has data that we're not looking for?
                    //maybe file is candy store inventory it has ID, name, type, amount, cost
                    if (lineSplit.Length != 5)
                    {
                        break;
                    }
                    else
                    {
                        using (
                            var connection = new SqlConnection("Server=dev;Database=BadExample;Trusted_Connection=True;")
                            )
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
                    file.Close();
                    counter = 0;
                }
            }
            //Store files on s3
            foreach (var thing in Directory.GetFiles("C:\\Users\\bkoenig\\Desktop\\Temp\\"))
            {
                var amazonClient = new AmazonS3Client(GetCredentials(), Amazon.RegionEndpoint.USEast1);
                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = "Attachments Bucket",
                    Key = thing,
                    FilePath = thing
                };
                PutObjectResponse response2 = amazonClient.PutObject(request);
            }
            //Delete files locally
            foreach (var thing in Directory.GetFiles("C:\\Users\\bkoenig\\Desktop\\Temp\\"))
            {
                File.Delete(thing);
            }
        }
        private static SessionAWSCredentials GetCredentials()
        {
            var stsClient = new AmazonSecurityTokenServiceClient("AWS Access Key Here", "AWS Secret Key Here");

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