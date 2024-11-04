using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Motoflex.Domain.Interfaces.Storage;

namespace Motoflex.Infrastructure.Storage
{
    public class AmazonStorage : IStorage
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _client;

        public AmazonStorage(IConfiguration configuration /*, IAmazonS3 client*/)
        {
            //_client = client ?? throw new ArgumentNullException(nameof(client));
            _bucketName = configuration["AWS:BucketName"] ?? "aplicacao-storage";
            Console.WriteLine($"BucketName: {_bucketName}");

            var accessKey = configuration["AWS:AccessKey"];
            var secretKey = configuration["AWS:SecretKey"];
            Console.WriteLine($"AccessKey: {accessKey}");
            Console.WriteLine($"SecretKey: {secretKey}");

            _client = new AmazonS3Client(
                accessKey,
                secretKey,
                RegionEndpoint.USEast2);
        }

        public async Task<string> UploadFile(Stream fileStream, string keyName)
        {
            ArgumentNullException.ThrowIfNull(fileStream);
            ArgumentException.ThrowIfNullOrWhiteSpace(keyName);

            try
            {
                var tranUtility = new TransferUtility(_client);
                await tranUtility.UploadAsync(fileStream, _bucketName, keyName);
                // _client.Config.RegionEndpoint.SystemName
                return $"https://{_bucketName}.s3.{RegionEndpoint.USEast2.SystemName}.amazonaws.com/{keyName}";
            }
            catch (AmazonS3Exception ex) when (ex.ErrorCode == "InvalidAccessKeyId" || ex.ErrorCode == "InvalidSecurity")
            {
                throw new UnauthorizedAccessException("Check the provided AWS Credentials.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while uploading file.", ex);
            }
        }
    }
}
