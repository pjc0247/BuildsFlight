Create your custom applications with C# API
----
__BuildsFlight__ also provides C# api to create your custom launcher or management tools.


Initialization
----
There's two ways for initialize __BuildsFlight__.<br>
<br>
가장 일반적인 방법은 __AWS__의 인증 정보를 사용해서 초기화하는 것이며, 이 경우에는 `BuildsFlight`의 모든 API가 사용 가능 상태가 됩니다.
```cs
BuildsFlight.Init(
    "ACCESS_KEY", "ACCESS_SECRET",
    "REGION_NAME",
    "BUCKET_NAME");
```

만약 배포할 앱에 인증 정보를 포함하기가 부담스러운 경우, __읽기 전용__으로 초기화하는 방법이 있습니다.<br>
이는 단순히 `BuildsFlight`의 인덱스파일의 경로만을 가지고 초기화를 수행합니다. 이 경우 __읽기__ API만 사용이 가능합니다.

```cs
BuildsFlight.Init("https://s3.ap-northeast-2.amazonaws.com/s3aad/buildsflight_index.json");
```


Create new Application
----
```cs
// create new
var app = BuildsFlight.CreateApp("test");
```


Query Application data
----
```cs
// returns application object which is already registered
var app = BuildsFlight.GetApp("test");
if (app == null)
    ; // No such app named 'test'
```
```cs
Console.WriteLine($"[{app.Name}]");
Console.WriteLine($"  created_at : {app.CreatedAt}");
Console.WriteLine($"  target_ver : {app.TargetVersion}");
Console.WriteLine($"  num_builds : {app.Builds.Count}");

foreach( var build in app.Builds)
{
    Console.WriteLine($"      - {build.Version}");
    Console.WriteLine($"          url : {build.Url}");
    Console.WriteLine($"          created_at : {build.CreatedAt}");
    Console.WriteLine($"          build_note : {build.Note}");
}
```


Add a new build
----
```cs
app.AddBuild("1.0.0", "download-url", "release-note");
```
`download-url`은 패키지 파일이 업로드된 다운로드 주소를 입력합니다. 다운로드 주소는 `NAS` 또는 `S3` 등이 될 수 있습니다.<br>
<br>
__BuildsFlight__는 패키지 파일 자체를 업로드해서 URL를 생성하는 API를 제공하지 않습니다.<br>
따라서 `download-url`을 생성하는 코드는 직접 작성해야 하며, 아래는 `AWSSDK.S3`를 이용해 S3에 파일을 업로드하고 URL을 가져오는 코드입니다.
```cs
var s3 = new AmazonS3Client(AccessKey, AccessSecret, RegionName);
var s3Key = $"buildsflight/{appName}/{version}/package.zip";

var resp = s3.PutObject(new PutObjectRequest()
{
    FilePath = filepath,
    Key = s3Key,
    CannedACL = S3CannedACL.PublicRead,
    BucketName = BucketName
});
if (resp.HttpStatusCode != System.Net.HttpStatusCode.OK)
{
    Console.WriteLine("Failed to upload file.");
    return -1;
}

var s3FilePath =
    $"https://s3.{RegionName}.amazonaws.com/{BucketName}/{s3Key}";
```

만약 업로드한 빌드를, 테스트 타겟 버전으로 설정하고 싶다면 아래와같이 설정합니다.
```cs
app.SetTargetVersion("1.0.0");
```
