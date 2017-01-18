using System;

using BuildsFlightCore;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

// USAGE
//    bf_cli_add_build APP_NAME VERSION FILEPATH NOTE
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
				Console.WriteLine("   bf_cli_add_build APP_NAME VERSION FILEPATH NOTE");
				return -1;
			}

			var appName = args[0];
			var version = args[1];
			var filepath = args[2];
			var note = args[3];

			var app = BuildsFlight.GetApp(appName);
			if (app == null)
			{
				Console.WriteLine("app does not exist");
				return -1;
			}

            var s3 = new AmazonS3Client(
                CliBase.AccessKey, CliBase.AccessSecret,
                RegionEndpoint.GetBySystemName(CliBase.RegionName));
            var s3Key = $"buildsflight/{appName}/{version}/package.zip";

            var resp = s3.PutObject(new PutObjectRequest()
            {
                FilePath = filepath,
                Key = s3Key,
                CannedACL = S3CannedACL.PublicRead,
                BucketName = CliBase.BucketName
            });
            if (resp.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Failed to upload file.");
                return -1;
            }

            var s3FilePath =
                $"https://s3.{CliBase.RegionName}.amazonaws.com/{CliBase.BucketName}/{s3Key}";

            Console.WriteLine(s3FilePath);

			app.AddBuild(version, s3FilePath, note);

			return 0;
		}
	}
}
