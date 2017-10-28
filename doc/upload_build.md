How to upload a build to BuildsFlight
====

Prepare your package
----
__BuildsFlight__ 에 업로드할 빌드를 구성하는 방법을 설명합니다.<br>
<br>
Package must be extracted with `zip`. __BuildsFlight__ 는 빌드를 다운로드 받아 압축을 푼 뒤, 아래의 스크립트를 실행합니다.<br>
따라서 __반드시__ 아래 실행 스크립트를 `.zip` 파일에 포함시켜 주세요.
```
./run.bat
```

Run script can be written like below:
```
cat_vs_mouse.exe --your-custom-arg
```

Upload a build
----
```cs
var app = BuildsFlight.GetApp("cat_vs_mouse");

app.AddBuild("1.0.0", "DOWNLOAD+URL");
```


Set as a test version
----
테스트 버전은 테스터가 테스트해야할 대상 버전을 나타냅니다.<br>
만약 별도의 옵션 없이 런쳐를 실행하면 해당 버전이 자동으로 실행됩니다.

```cs
// 테스트 버전은 이미 존재하는 빌드 버전이어야 합니다.
//   만약 업로드 되지 않은 버전을 지정하면 익셉션이 발생합니다.
app.SetTargetVersion("1.0.0");
```
