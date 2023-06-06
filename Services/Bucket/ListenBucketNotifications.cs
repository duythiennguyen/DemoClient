using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel;
using MinIOClient.Global;
using Newtonsoft.Json;

namespace MinIOClient.Services.Bucket
{
    internal static class ListenBucketNotifications
    {
        // Listen for notifications from a specified bucket (a Minio-only extension)
        public static void Run(MinioClient minio,
            string bucketName = "my-bucket-name",
            List<EventType> events = null,
            string prefix = "",
            string suffix = "",
            bool recursive = true)
        {
            try
            {
                Console.WriteLine("Running example for API: ListenBucketNotifications");
                Console.WriteLine();
                events ??= new List<EventType> { EventType.ObjectCreatedAll };
                var args = new ListenBucketNotificationsArgs()
                    .WithBucket(bucketName)
                    .WithPrefix(prefix)
                    .WithEvents(events)
                    .WithSuffix(suffix);
                var observable = minio.ListenBucketNotificationsAsync(bucketName, events, prefix, suffix);

                var subscription = observable.Subscribe(
                    notification =>
                    {
                        Console.WriteLine($"Notification: {notification.json}");
                        var a = JsonConvert.DeserializeObject<ResultListenBucketNotifi>(notification.json);
                    },

                    ex => Console.WriteLine($"OnError: {ex}"),
                    () => Console.WriteLine("Stopped listening for bucket notifications\n"));

                // subscription.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
            }
        }
    }
}