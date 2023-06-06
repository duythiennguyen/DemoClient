using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Replication;

namespace MinIOClient.Services.Bucket
{
    public static class SetBucketReplication
    {
        private static string Bash(string cmd)
        {
            var escapedArgs = "";
            foreach (var str in new List<string>
                 {
                     "$", "(", ")", "{", "}",
                     "[", "]", "@", "#", "$",
                     "%", "&", "+"
                 })
                escapedArgs = cmd.Replace("str", "\\str");

            var fileName = "/bin/bash";
            var arguments = $"-c \"{escapedArgs}\"" +
                            "RedirectStandardOutput = true" +
                            "UseShellExecute = false" +
                            "CreateNoWindow = true";
            var startInfo = new ProcessStartInfo(fileName, arguments);
            using var process = Process.Start(startInfo);

            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }

        // Set Replication configuration for the bucket
        public static async Task Run(IMinioClient minio,
            string bucketName = "my-bucket-name",
            string destBucketName = "dest-bucket-name",
            string replicationRuleID = "my-replication-ID")
        {
            if (minio is null) throw new ArgumentNullException(nameof(minio));

            var setArgs = new SetVersioningArgs()
                .WithBucket(bucketName)
                .WithVersioningEnabled();
            await minio.SetVersioningAsync(setArgs).ConfigureAwait(false);
            setArgs = new SetVersioningArgs()
                .WithBucket(destBucketName)
                .WithVersioningEnabled();
            await minio.SetVersioningAsync(setArgs).ConfigureAwait(false);
            var schema = "http://";
            string serverEndPoint;
            string accessKey;
            string secretKey;

            // serverEndPoint = "play.min.io";
            // accessKey = "MDXOa5sHRTvsPutE";
            // secretKey = "xFgnDvn0PQEiNodvWzYzUrr1NXZhYGfv";
            serverEndPoint = "minhlongq3.southeastasia.cloudapp.azure.com";
            accessKey = "5teAeQzSBosBcthG";
            secretKey = "9D7eZo4uqbqaH3Cuf0PRtUvWYSs9Ol7R";

            schema = "http://";

            var cmdFullPathMC = Bash("which mc").TrimEnd('\r', '\n', ' ');
            var cmdAlias = cmdFullPathMC + " alias list | egrep -B1 \"" +
                           schema + serverEndPoint + "\" | grep -v URL";
            var alias = Bash(cmdAlias).TrimEnd('\r', '\n', ' ');

            var cmdRemoteAdd = cmdFullPathMC + " admin bucket remote add " +
                               alias + "/" + bucketName + "/ " + schema +
                               accessKey + ":" + secretKey + "@" +
                               serverEndPoint + "/" + destBucketName +
                               " --service replication --region us-east-1";

            var arn = Bash(cmdRemoteAdd).Replace("Remote ARN = `", "").Replace("`.", "");

            var rule =
                new ReplicationRule(
                    new DeleteMarkerReplication(DeleteMarkerReplication.StatusDisabled),
                    new ReplicationDestination(null, null,
                        "arn:aws:s3:::" + destBucketName,
                        null, null, null, null),
                    new ExistingObjectReplication(ExistingObjectReplication.StatusEnabled),
                    new RuleFilter(null, null, null),
                    new DeleteReplication(DeleteReplication.StatusDisabled),
                    1,
                    replicationRuleID,
                    "",
                    new SourceSelectionCriteria(new SseKmsEncryptedObjects(
                        SseKmsEncryptedObjects.StatusEnabled)),
                    ReplicationRule.StatusEnabled
                );
            var rules = new Collection<ReplicationRule>
        {
            rule
        };
            var repl = new ReplicationConfiguration(arn, rules);

            await minio.SetBucketReplicationAsync(
                new SetBucketReplicationArgs()
                    .WithBucket(bucketName)
                    .WithConfiguration(repl)
            ).ConfigureAwait(false);
        }
    }
}