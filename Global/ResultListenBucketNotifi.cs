
using Newtonsoft.Json;
namespace MinIOClient.Global
{
    public class ResultListenBucketNotifi
    {
        public List<Record> Records { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Bucket
    {
        public string name { get; set; }
        public OwnerIdentity ownerIdentity { get; set; }
        public string arn { get; set; }
    }

    public class Object
    {
        public string key { get; set; }
        public int size { get; set; }
        public string eTag { get; set; }
        public string contentType { get; set; }
        public UserMetadata userMetadata { get; set; }
        public string versionId { get; set; }
        public string sequencer { get; set; }
    }

    public class OwnerIdentity
    {
        public string principalId { get; set; }
    }

    public class Record
    {
        public string eventVersion { get; set; }
        public string eventSource { get; set; }
        public string awsRegion { get; set; }
        public DateTime eventTime { get; set; }
        public string eventName { get; set; }
        public UserIdentity userIdentity { get; set; }
        public RequestParameters requestParameters { get; set; }
        public ResponseElements responseElements { get; set; }
        public S3 s3 { get; set; }
        public Source source { get; set; }
    }

    public class RequestParameters
    {
        public string principalId { get; set; }
        public string region { get; set; }
        public string sourceIPAddress { get; set; }
    }

    public class ResponseElements
    {
        [JsonProperty("content-length")]
        public string contentlength { get; set; }

        [JsonProperty("x-amz-id-2")]
        public string xamzid2 { get; set; }

        [JsonProperty("x-amz-request-id")]
        public string xamzrequestid { get; set; }

        [JsonProperty("x-minio-deployment-id")]
        public string xminiodeploymentid { get; set; }

        [JsonProperty("x-minio-origin-endpoint")]
        public string xminiooriginendpoint { get; set; }
    }

    public class S3
    {
        public string s3SchemaVersion { get; set; }
        public string configurationId { get; set; }
        public Bucket bucket { get; set; }
        public Object @object { get; set; }
    }

    public class Source
    {
        public string host { get; set; }
        public string port { get; set; }
        public string userAgent { get; set; }
    }

    public class UserIdentity
    {
        public string principalId { get; set; }
    }

    public class UserMetadata
    {
        [JsonProperty("content-type")]
        public string contenttype { get; set; }
    }



}