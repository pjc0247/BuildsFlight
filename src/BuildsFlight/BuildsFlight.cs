using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Net;
using System.IO;

using S3AAD;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildsFlightCore
{
    enum InitType
    {
        NotInitialized,
        FullAccess,
        ReadOnly
    }

	public class BuildsFlight
	{
        private static InitType initType = InitType.NotInitialized;

		private static readonly string IndexFileName = "buildsflight_index.json";

        private static string IndexFileUrl;
		private static string BucketName;

        public static void Init(string indexFileUrl)
        {
            IndexFileUrl = indexFileUrl;

            initType = InitType.ReadOnly;
        }
		public static void Init(string accessKey, string accessSecret, string regionName, string bucketName)
		{
			BucketName = bucketName;
			S3DB.Init(accessKey, accessSecret, regionName);

            initType = InitType.FullAccess;
		}

		public static void InitIndexfile()
		{
            if (initType != InitType.FullAccess)
                throw new InvalidOperationException("Not initialized with access_token");

			var index = S3DB.CreateDocument(BucketName, IndexFileName);

			index["data"] = JsonConvert.SerializeObject(new IndexFile()
			{
				Apps = new Dictionary<string, AppInfo>() { }
			});

			index.Update();
		}

		internal static void UpdateApp(string name, AppInfo appInfo)
		{
            if (initType != InitType.FullAccess)
                throw new InvalidOperationException("Not initialized with access_token");

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
            if (initType != InitType.FullAccess)
                throw new InvalidOperationException("Not initialized with access_token");

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
            if (initType == InitType.NotInitialized)
                throw new InvalidOperationException("not initialized");

            var json = "";

            if (initType == InitType.ReadOnly)
            {
                var resp = HttpWebRequest.Create(IndexFileUrl).GetResponse();
                var indexFileJson = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(indexFileJson);
                json = (string)((JObject)dic["data"]).GetValue("data");
            }
            else
            {
                var index = S3DB.FindDocument(BucketName, IndexFileName);
                if (index == null)
                    throw new InvalidOperationException("indexfile not exists in this bucket : " + BucketName);

                json = (string)index["data"];
            }

			var indexObj = JsonConvert.DeserializeObject<IndexFile>(json);

			var apps = indexObj.Apps;
			if (apps.ContainsKey(name) == false)
				return null;
			
			return apps[name];
		}
	}
}
