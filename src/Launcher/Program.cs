using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Linq;
using System.Net;

using BuildsFlightCore;

namespace Launcher
{
	class MainClass
	{
        private static void ErrorClose(string msg)
        {
            Console.WriteLine(msg);
            Console.Read();

            Environment.Exit(-1);
        }
        private static void DownloadFile(string url, string path)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, path);
        }

		public static void Main(string[] args)
		{
            Console.WriteLine("args : " + string.Join(", ", args));

			if (File.Exists("buildsflight.app") == false)
                ErrorClose("`buildsflight.app` file not found");

			if (Directory.Exists("builds") == false)
				Directory.CreateDirectory("builds");

			var appFile = File.ReadAllText("buildsflight.app");
			var appFileLines = appFile.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			if (appFileLines.Length != 2)
				ErrorClose("malformed .app file");

			BuildsFlight.Init(appFileLines[1]);
			var appId = appFileLines[0];
			var app = BuildsFlight.GetApp(appId);
            if (app == null)
                ErrorClose("app not found : " + appId);

            var targetVersion = app.TargetVersion ?? "1.0.0";

            if (args.Contains("--select-version"))
            {
                Console.WriteLine("[Select Version]");
                int idx = 1;
                foreach(var build in app.Builds.OrderBy(x => x.Version))
                {
                    Console.WriteLine($"   {idx} - {build.Version}");
                    idx++;
                }

                while (true)
                {
                    Console.Write(">> ");
                    try
                    {
                        var input = int.Parse(Console.ReadLine());
                        targetVersion = app.Builds[input - 1].Version;
                        break;
                    }
                    catch (Exception) { }
                }
            }

			Console.WriteLine("TargetVersion : " + targetVersion);

			var gameDir = $".\\builds\\{targetVersion}\\";
			if (Directory.Exists(gameDir) == false)
			{
				Directory.CreateDirectory(gameDir);

                var build = app.Builds.First(x => x.Version == targetVersion);

                Console.WriteLine("DownloadBuild : " + build.Url, gameDir + "package.zip");
                DownloadFile(build.Url, gameDir + "package.zip");

                ZipFile.ExtractToDirectory(gameDir + "package.zip", gameDir);
            }

			Process.Start(new ProcessStartInfo()
			{
				FileName = "run.bat", 
				WorkingDirectory = gameDir
			});
		}
	}
}
   