using System.Threading.Tasks;
using CliFx;
using Microwave.Commands;
using Microwave.Localization;
using TomatoKnishes.Internals;

namespace Microwave
{
    public static class Program
    {
        public static bool Headless { get; private set; }

        public static DownloadProfile Profile { get; set; } = DownloadProfile.Release;

        static Program()
        {
            Knishes.Localizer.AddProvider<MicrowaveLocalization, LocalizationType>();
        }

        public static async Task<int> Main(string[] args)
        {
            if (args.Length > 0)
            {
                Headless = true;

                return await new CliApplicationBuilder()
                    .AddCommandsFromThisAssembly()
                    .Build()
                    .RunAsync();
            }

            Headless = false;
            // await new InstallCommand().ExecuteAsync(null);
            await new MicrowaveWindow().WriteFullConsole();

            return 0;
        }
    }
}