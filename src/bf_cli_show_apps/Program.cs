using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BuildsFlightCore;

namespace bf_cli_show_apps
{
    class Program
    {
        static void Main(string[] args)
        {
            CliBase.Init();

            while (true)
            {
                Console.Write("AppName : ");
                var name = Console.ReadLine();

                var app = BuildsFlight.GetApp(name);
                if (app == null)
                {
                    Console.WriteLine("App not found");
                    continue;
                }

                Console.WriteLine(); Console.WriteLine();

                Console.WriteLine($"[{app.Name}]");
                Console.WriteLine($"  created_at : {app.CreatedAt}");
                Console.WriteLine($"  target_ver : {app.TargetVersion}");
                Console.WriteLine($"  num_builds : {app.Builds.Count}");

                foreach( var build in app.Builds)
                {
                    Console.WriteLine($"      - {build.Version}");
                    Console.WriteLine($"          url : {build.Url}");
                    Console.WriteLine($"          created_at : {build.CreatedAt}");
                    Console.WriteLine($"          build_note : {build.Note}");
                }

                Console.WriteLine(); Console.WriteLine();
            }
        }
    }
}
