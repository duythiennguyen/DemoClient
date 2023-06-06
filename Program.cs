// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using Minio;
using Minio.DataModel;
using MinIOClient.Enums;
using MinIOClient.Global;
using MinIOClient.Services.Bucket;
using MinIOClient.Services.ObjectFile;

try
{

    var endpoint = "minhlongq3.southeastasia.cloudapp.azure.com";
    var accessKey = "6OPr2lC8XOaQjLxM";
    var secretKey = "OoXQ2czEQRlPZhiovEskYjIeHPz43ZwV";
    var port = 9000;


    using var minioClient = new MinioClient()
               .WithEndpoint(endpoint, port)
               .WithCredentials(accessKey, secretKey)
               .WithSSL(false)
               .Build();





    var bucketName = "bucket2";

    #region Bucket

    // var getListBucketsTask = await minioClient.ListBucketsAsync().ConfigureAwait(false);
    // foreach (var bucket in getListBucketsTask.Buckets)
    // {
    //     Console.WriteLine(bucket.Name + " " + bucket.CreationDateDateTime);
    // }

    // bool found = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
    // if (!found)
    // {
    //     await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
    //}
    #endregion


    #region Policy

    // var policy = ListPolicy.Get(EPolicy.Public, bucketName);
    // await minioClient.SetPolicyAsync(new SetPolicyArgs().WithBucket(bucketName).WithPolicy(policy));
    // var a = await minioClient.GetPolicyAsync(new GetPolicyArgs().WithBucket(bucketName));

    #endregion

    #region Object
    var objectName = "myobject.mp4";
    var filePath = @"C:\Users\azureuser\Downloads\ascvasv.mp4";

    // await PutObject.Run(minioClient, bucketName, objectName, filePath).ConfigureAwait(false);

    // var obj = await minioClient.GetObjectAsync(new GetObjectArgs().WithBucket("sanpham")
    // .WithObject("Project1/Bo-1/HoaVan-1/1/video/pexels-stijn-dijkstra-13008655-2160x3240-25fps.mp4").WithFile(filePath)); // chua duoc

    // var presigneGetdUrl = await minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs().WithBucket("bucket1")
    // .WithObject("video/All Quiet On The Western Front (2022) [1080p] [WEBRip] [5.1] [YTS.MX]/All.Quiet.On.The.Western.Front.2022.1080p.WEBRip.x264.AAC5.1-[YTS.MX].mp4")
    // .WithExpiry(60));//giay


    // var presignedPutUrl = await minioClient.PresignedPutObjectAsync(new PresignedPutObjectArgs().WithBucket(bucketName).WithObject("test.txt").WithExpiry(60));
    // var httpClient = new System.Net.Http.HttpClient();
    // var fileContent = new System.Net.Http.StringContent("This is the content of my object 22222222");
    // var response = await httpClient.PutAsync(presignedPutUrl, fileContent);

    // var policy = new PostPolicy();
    // policy.SetKey("Project1");
    // policy.SetBucket("sanpham");
    // policy.SetExpires(DateTime.UtcNow.AddMinutes(100));
    // var presignedPostPolicyUrl = await minioClient.PresignedPostPolicyAsync(policy);

    // Dictionary<string, string> formData = new Dictionary<string, string>(presignedPostPolicyUrl.Item2);
    // string curlCommand = "curl ";
    // foreach (KeyValuePair<string, string> pair in formData)
    // {
    //     curlCommand = curlCommand + " -F " + pair.Key + "=" + pair.Value;
    // }
    // curlCommand = curlCommand + " -F file=R.png http://minhlongq3.southeastasia.cloudapp.azure.com:9000/sanpham/Project1";
    // Console.WriteLine(curlCommand);

    // var a = new PresignedPutObjectArgs()
    //                                      .WithBucket("sanpham")
    //                                      .WithObject("project0/a.png")
    //                                      .WithExpiry(60 * 60 * 24);
    // String url = await minioClient.PresignedPutObjectAsync(a);
    GetPolicyArgs argsGetPolicy = new GetPolicyArgs()
                 .WithBucket("sanpham");
    String policyJson = await minioClient.GetPolicyAsync(argsGetPolicy);
    Console.WriteLine("Current policy: " + policyJson);

    // #endregion

    // #region select object 
    // // List objects from 'my-bucketname'
    // List<Item> listResult = new List<Item>();

    // ListObjectsArgs args2 = new ListObjectsArgs()
    //                           .WithBucket("sanpham")
    //                           .WithPrefix("/")
    //                           .WithRecursive(true);
    // IObservable<Item> observable = minioClient.ListObjectsAsync(args2);
    // ManualResetEvent completedEvent = new ManualResetEvent(false);
    // IDisposable subscription = observable.Subscribe(
    //         item =>
    //         {
    //             // Console.WriteLine("OnNext: {0}", item.Key);
    //             listResult.Add(item);
    //         },
    //         ex => Console.WriteLine("OnError: {0}", ex.Message),
    //         () => completedEvent.Set()
    // );
    // completedEvent.WaitOne();
    // // Giải phóng tài nguyên
    // subscription.Dispose();
    // completedEvent.Dispose();

    // foreach (var item in listResult)
    // {
    //     var extension = Path.GetExtension(item.Key);
    //     Console.WriteLine("OnNext: {0}", item.Key);
    // }
    #endregion

    #region Replicate to other server
    // var reSchema = "http";
    // var reUserName = "minioadmin";
    // var rePassword = "minioadmin";
    // var reServer = "127.0.0.1:9000";
    // var reBucket = "bucket2-replication";
    // var alias = "ALIAS";

    // ProcessStartInfo startInfo = new ProcessStartInfo();
    // startInfo.FileName = "cmd.exe";
    // startInfo.Arguments = String.Format(@"/C mc replicate add --remote-bucket {0}://{1}:{2}@{3}/{4} --replicate ""delete,delete-marker,existing-objects"" {5}/{6}",
    // reSchema, reUserName, rePassword, reServer, reBucket, alias, bucketName); 

    // Process process = new Process();
    // process.StartInfo = startInfo;
    // process.Start();
    // process.WaitForExit();
    #endregion


    #region Noti Bucket Change
    ListenBucketNotifications.Run(minioClient, bucketName, new List<EventType> { EventType.ObjectCreatedAll });
    ListenBucketNotifications.Run(minioClient, bucketName, new List<EventType> { EventType.ObjectRemovedAll });

    #endregion

    Console.ReadLine();
}
catch (System.Exception ex)
{

    throw;
}




