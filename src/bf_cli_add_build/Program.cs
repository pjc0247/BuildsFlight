using System;

using BuildsFlightCore;

// USAGE
//    bf_cli_add_build APP_NAME VERSION URL NOTE
namespace bf_cli_add_build
{
	class MainClass
	{
		public static int Main(string[] args)
		{
			CliBase.Init();

			if (args.Length < 4)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("   bf_cli_add_build APP_NAME VERSION URL NOTE");
				return -1;
			}

			var appName = args[0];
			var version = args[1];
			var url = args[2];
			var note = args[3];

			var app = BuildsFlight.GetApp(appName);
			if (app == null)
			{
				Console.WriteLine("app does not exist");
				return -1;
			}

			app.AddBuild(version, url, note);

			return 0;
		}
	}
}
