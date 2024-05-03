using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Domain.Interfaces.Storage;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Storages
{
    public class AmazonStorage : IStorage
    {
        private readonly string _bucketName = "aplicacao-storage";
        private readonly AmazonS3Client _client;

        public AmazonStorage(IConfiguration configuration)
        {
            _client = new AmazonS3Client(
                configuration.GetConnectionString("AWSAccessKey"),
                configuration.GetConnectionString("AWSSecretKey"),
                RegionEndpoint.USEast2
            );
        }

        public async Task<string> UploadFile(Stream fileStream, string keyName)
        {
            try
            {
                using (TransferUtility tranUtility = new TransferUtility(_client))
                {
                    await tranUtility.UploadAsync(fileStream, _bucketName, keyName);
                    return "https://aplicacao-storage.s3.us-east-2.amazonaws.com/" + keyName;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }
    }

}
