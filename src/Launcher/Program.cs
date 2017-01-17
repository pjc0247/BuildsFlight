using System;
using System.IO;
using System.Diagnostics;

using BuildsFlightCore;

namespace Launcher
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			BuildsFlight.Init(
				"AKIAJNLHNGLVGVZ32ZXA", "lC29YKN28u/f4IPeU3CCb+3m6X/D9bcv6r0BKFTS", "ap-northeast-2",
				"s3aad");
			
			if (File.Exists("buildsflight.app"))
			{
				Console.WriteLine("`buildsflight.app` file not found");
				return;
			}
			if (Directory.Exists("builds") == false)
				Directory.CreateDirectory("builds");

			var appId = "test2";//File.ReadAllText("buildsflight.app");
			var app = BuildsFlight.GetApp(appId);

			Console.WriteLine("TargetVersion : " + app.TargetVersion);

			var gameDir = $".\\builds\\{app.TargetVersion}\\";
			if (Directory.Exists(gameDir) == false)
			{
				Directory.CreateDirectory(gameDir);

			}

			Process.Start(new ProcessStartInfo()
			{
				FileName = gameDir + "run.bat",
				WorkingDirectory = gameDir
			});
		}
	}
}
