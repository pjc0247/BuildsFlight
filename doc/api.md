API를 사용하여 커스텀 앱 작성하기
----

제공되는 API를 이용하여 커스텀 런쳐, 또는 빌드 관리 툴을 작성할 수 있습니다.


초기화하기
----
`BuildsFlight`는 두가지 방식의 초기화 방법을 제공합니다.<br>
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


앱 생성하기
----
```cs
// 새로 만들기
var app = BuildsFlight.CreateApp("test");
```

앱 정보 가져오기
----
```cs
// 이미 만들어진 앱 가져오기
var app = BuildsFlight.GetApp("test");
if (app == null)
    ; // 앱 찾을 수 없음
```


빌드 추가하기
----
```cs
app.AddBuild("1.0.0", "download-url", "release-note");
```

만약 업로드한 빌드를, 테스트 타겟 버전으로 설정하고 싶다면 아래와같이 설정합니다.
```cs
app.SetTargetVersion("1.0.0");
```
