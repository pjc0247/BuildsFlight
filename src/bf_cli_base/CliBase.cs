using System;

using BuildsFlightCore;


public class CliBase
{
    public static string AccessKey;
    public static string AccessSecret;
    public static string RegionName;
    public static string BucketName;

	public static void Init()
	{
        var target = EnvironmentVariableTarget.User | EnvironmentVariableTarget.Process;

		AccessKey = Environment.GetEnvironmentVariable("BF_ACCESS_KEY", target);
		AccessSecret = Environment.GetEnvironmentVariable("BF_ACCESS_SECRET", target);
		RegionName = Environment.GetEnvironmentVariable("BF_REGION", target) ?? "ap-northeast-2";
		BucketName = Environment.GetEnvironmentVariable("BF_BUCKET", target);

		if (string.IsNullOrEmpty(AccessKey) || string.IsNullOrEmpty(AccessSecret))
		{
			Console.WriteLine("BF_ACCESS_KEY or BF_ACCESS_SECRET not found");
			Environment.Exit(-1);
		}
		if (string.IsNullOrEmpty(BucketName))
		{
			Console.WriteLine("BF_BUCKET not found");
			Environment.Exit(-1);
		}

		BuildsFlight.Init(AccessKey, AccessSecret, RegionName, BucketName);
	}
}
