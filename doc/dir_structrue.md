런쳐 구조
====

```
CatVsMouse
 * launcher.exe
 * buildsflight.app
 / builds
      / 1.0.0
       * run.bat
      / 1.1.0
       * run.bat
      / 0.1.0.pre
       * run.bat
```

* __launcher.exe__ : 누르면 게임이 시작됩니다.
* __buildsflight.app__ : 앱 정보
* __builds__ : 각각의 게임 빌드 폴더

<br>
```
처음에는 3개의 파일만 배포하며, builds 폴더 하위는 알아서 생성됨
```
