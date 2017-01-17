using System;
using System.Linq;
using System.Collections.Generic;

namespace BuildsFlightCore
{
	public class AppInfo
	{
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }

		public string LatestVersion { get; set; }
		public string TargetVersion { get; set; }

		public List<BuildInfo> Builds { get; set; }

		public void SetTargetVersion(string version)
		{
			if (Builds.Any(x => x.Version == version) == false)
				throw new ArgumentException("version not exists");

			TargetVersion = version;

			BuildsFlight.UpdateApp(Name, this);
		}
		public void AddBuild(string version, string url, string note)
		{
			if (Builds.Any(x => x.Version == version))
				throw new ArgumentException("version already exists");
			
			Builds = Builds.Concat(
				new BuildInfo[]
				{
					new BuildInfo(){
						CreatedAt = DateTime.Now,
						Url = url,
						Version = version,
						Note = note
					}
				})
				.ToList();

			BuildsFlight.UpdateApp(Name, this);
		}
	}
}
