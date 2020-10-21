using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;

namespace Commandante
{
    public class Program
    {
        private const string RegistryKeyName = "HKEY_CURRENT_USER\\Software\\CrazyPokemonDev\\Commandante";
        internal const string DefaultJwtSecret = "IHaveNoSecrets";
        internal static string JwtSecret = (string)Registry.GetValue(RegistryKeyName, "JwtSecret", DefaultJwtSecret);
        internal static string ServiceName = (string)Registry.GetValue(RegistryKeyName, "ServiceName", null);
        private static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Commandante");
        public static readonly DirectoryInfo AppData =
            Directory.CreateDirectory(AppDataPath);
        internal static readonly string UpdaterExecutableFilePath = Path.Combine(AppDataPath, "Updater\\CommandanteUpdater.exe");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
