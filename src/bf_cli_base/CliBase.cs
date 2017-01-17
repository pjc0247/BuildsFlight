using System;

using BuildsFlightCore;


public class CliBase
{
	public static void Init()
	{
		var appKey = Environment.GetEnvironmentVariable("BF_ACCESS_KEY");
		var appSecret = Environment.GetEnvironmentVariable("BF_ACCESS_SECRET");
		var region = Environment.GetEnvironmentVariable("BF_REGION") ?? "ap-northeast-2";
		var bucketName = Environment.GetEnvironmentVariable("BF_BUCKET");

		if (string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(appSecret))
		{
			Console.WriteLine("BF_ACCESS_KEY or BF_ACCESS_SECRET not found");
			Environment.Exit(-1);
		}
		if (string.IsNullOrEmpty(bucketName))
		{
			Console.WriteLine("BF_BUCKET not found");
			Environment.Exit(-1);
		}

		BuildsFlight.Init(appKey, appSecret, region, bucketName);
	}
}
