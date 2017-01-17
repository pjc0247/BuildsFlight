using System;

using BuildsFlightCore;


public class CliBase
{
	public static void Init()
	{
        var target = EnvironmentVariableTarget.User | EnvironmentVariableTarget.Process;

		var appKey = Environment.GetEnvironmentVariable("BF_ACCESS_KEY", target);
		var appSecret = Environment.GetEnvironmentVariable("BF_ACCESS_SECRET", target);
		var region = Environment.GetEnvironmentVariable("BF_REGION", target) ?? "ap-northeast-2";
		var bucketName = Environment.GetEnvironmentVariable("BF_BUCKET", target);

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
