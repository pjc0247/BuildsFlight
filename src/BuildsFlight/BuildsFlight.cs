using System;
using System.Dynamic;
using System.Collections.Generic;

using S3AAD;

using Newtonsoft.Json;

namespace BuildsFlightCore
{
	public class BuildsFlight
	{
		private static readonly string IndexFileName = "buildsflight_index.json";

		private static string BucketName;

		public static void Init(string accessKey, string accessSecret, string regionName, string bucketName)
		{
			BucketName = bucketName;
			S3DB.Init(accessKey, accessSecret, regionName);
		}

		public static void InitIndexfile()
		{
			var index = S3DB.CreateDocument(BucketName, IndexFileName);

			index["data"] = JsonConvert.SerializeObject(new IndexFile()
			{
				Apps = new Dictionary<string, AppInfo>() { }
			});

			index.Update();
		}

		internal static void UpdateApp(string name, AppInfo appInfo)
		{
			var index = S3DB.FindDocument(BucketName, IndexFileName);
			if (index == null)
				throw new InvalidOperationException("indexfile not exists in this bucket : " + BucketName);

			var json = (string)index["data"];
			var indexObj = JsonConvert.DeserializeObject<IndexFile>(json);
			var apps = indexObj.Apps;

			apps[name] = appInfo;

			index["data"] = JsonConvert.SerializeObject(indexObj);
			index.Update();
		}

		public static AppInfo CreateApp(string name)
		{
			var index = S3DB.FindDocument(BucketName, IndexFileName);
			if (index == null)
				throw new InvalidOperationException("indexfile not exists in this bucket : " + BucketName);

			var json = (string)index["data"];
			var indexObj = JsonConvert.DeserializeObject<IndexFile>(json);
			var apps = indexObj.Apps;

			var appInfo = new AppInfo();
			apps.Add(name, appInfo);

			appInfo.CreatedAt = DateTime.Now;
			appInfo.Name = name;
			appInfo.Builds = new List<BuildInfo> {
				new BuildInfo() {
					Version = "0.0.0",
					Url = "",
					Note = ""
				}
			};

			index["data"] = JsonConvert.SerializeObject(indexObj);
			index.Update();

			return appInfo;
		}
		public static AppInfo GetApp(string name)
		{
			var index = S3DB.FindDocument(BucketName, IndexFileName);
			if (index == null)
				throw new InvalidOperationException("indexfile not exists in this bucket : " + BucketName);

			var json = (string)index["data"];
			var indexObj = JsonConvert.DeserializeObject<IndexFile>(json);

			var apps = indexObj.Apps;
			if (apps.ContainsKey(name) == false)
				return null;
			
			return apps[name];
		}
	}
}
