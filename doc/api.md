

__init__
```cs
BuildsFlight.Init(
    "ACCESS_KEY", "ACCESS_SECRET",
    "REGION_NAME",
    "BUCKET_NAME");
```

```cs
AppInfo app = null;

// 새로 만들기
app = BuildsFlight.CreateApp("test");

// 이미 만들어진 앱 가져오기
app = BuildsFlight.GetApp("test");
if (app == null)
    ; // 앱 찾을 수 없음
```

```cs
app.AddBuild("1.0.0", "download-url", "release-note");

app.SetTargetVersion("1.0.0");
```
