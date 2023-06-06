using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using Minio;
using Minio.DataModel;

namespace MinIOClient.Services.ObjectFile
{
    public class PutObject
    {
        private const int MB = 1024 * 1024;

        // Put an object from a local stream into bucket
        public static async Task Run(IMinioClient minio,
            string bucketName = "my-bucket-name",
            string objectName = "my-object-name",
            string fileName = "location-of-file",
            IServerSideEncryption sse = null)
        {
            try
            {
                ReadOnlyMemory<byte> bs = await File.ReadAllBytesAsync(fileName).ConfigureAwait(false);
                Console.WriteLine("Running example for API: PutObjectAsync");
                using var filestream = bs.AsStream();

                var fileInfo = new FileInfo(fileName);
                var metaData = new Dictionary<string, string>
            {
                { "Test-Metadata", "Test  Test" }
            };
                var args = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject("/video/" + objectName)
                    .WithStreamData(filestream)
                    .WithObjectSize(filestream.Length)
                    .WithContentType("application/octet-stream")
                    .WithHeaders(metaData)
                    .WithServerSideEncryption(sse);
                await minio.PutObjectAsync(args).ConfigureAwait(false);

                Console.WriteLine($"Uploaded object {objectName} to bucket {bucketName}");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
            }
        }
    }
}