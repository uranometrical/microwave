using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microwave.GitHub;
using Microwave.Localization;
using Newtonsoft.Json;
using Spectre.Console;
using TomatoKnishes.Internals;
using TomatoKnishes.SpectreFx.Identities;

namespace Microwave.Commands
{
    [Command("install", Description = "Installs the client.")]
    public class InstallCommand : ICommand, ICommandIdentity
    {
        public const string LatestReleaseUrl = "https://api.github.com/repos/Uranometrical/Constellar/releases/latest";

        public string CommandName => "Install Client";

        public string CommandDescription => "Installs the latest stable release of Constellar.";

        [CommandOption("profile", 'p', Description = "Download profile (\"Release\" or \"BleedingEdge\").")]
        public DownloadProfile Profile
        {
            get => Program.Profile;

            set => Program.Profile = value;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            if (!Program.Headless)
                AskForInput();

            // todo: output path and minecraft/multimc profiles

            AnsiConsole.WriteLine("Installing from release channel: " + Profile);

            switch (Profile)
            {
                case DownloadProfile.Release:
                    await DownloadStable();
                    break;

                case DownloadProfile.BleedingEdge:
                    throw new NotImplementedException("Bleeding edge channel is not yet implemented.");

                default:
                    throw new Exception("Invalid profile: " + Profile);
            }

            AnsiConsole.WriteLine("Download complete!");
        }

        public void AskForInput()
        {
            // Prompt for profile.
            Profile = AnsiConsole.Prompt(
                new SelectionPrompt<DownloadProfile>()
                    .Title("Select a download profile:")
                    .AddChoices(DownloadProfile.Release, DownloadProfile.BleedingEdge)
            );
        }

        public static async Task DownloadStable()
        {
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Constellar/v0.1.0-alpha");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            HttpResponseMessage releaseResponse = await httpClient.GetAsync(LatestReleaseUrl);
            string releaseResponseJson = await releaseResponse.Content.ReadAsStringAsync();
            RepositoryRelease latestRelease = JsonConvert.DeserializeObject<RepositoryRelease>(releaseResponseJson);

#if DEBUG
            foreach (Asset asset in latestRelease.Assets)
                AnsiConsole.WriteLine(asset.Name + " " + asset.BrowserDownloadUrl);
#endif

            // TODO: Why the hell is this called ConstellarMain GoodPro?
            // Find .jar file with a name pattern consistent of "ConstellarMain-" at the start and ".jar" at the end. Ignores the version.
            string constellarUrl = latestRelease.Assets
                .Find(asset => asset.Name.StartsWith("ConstellarMain-") && asset.Name.EndsWith(".jar"))
                .BrowserDownloadUrl;

            AnsiConsole.WriteLine(constellarUrl);

            if (constellarUrl == string.Empty)
            {
                AnsiConsole.Markup("[color red]Release not found![/]");
                await Task.CompletedTask;
            }

            try
            {
                await AnsiConsole.Console.Progress()
                    .Columns(
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                        new RemainingTimeColumn(),
                        new SpinnerColumn()
                    )
                    .StartAsync(async ctx => await DownloadFile(ctx, constellarUrl));
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
            }

            // TODO: bruh
            AnsiConsole.Ask<string>("Press enter to continue...");
        }

        public static async Task DownloadFile(ProgressContext ctx, string uri)
        {
            ProgressTask progressTask = ctx.AddTask(
                Knishes.GetLocalizedText(LocalizationType.DownloadingConstellar),
                new ProgressTaskSettings
                {
                    AutoStart = false
                });

            // stealing my horrible code from SoundsGoodToMe:
            // https://stackoverflow.com/a/56091135
            const int bufferSize = 8192;

            // never sends
            AnsiConsole.WriteLine("pre try");

            try
            {
                HttpClient client = new();
                using HttpResponseMessage response = await client.GetAsync(uri,
                    HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                // Set the max value of the progress task to the number of bytes
                progressTask.MaxValue(response.Content.Headers.ContentLength ?? 0);
                // Start the progress task
                progressTask.StartTask();

                //todo: localization
                AnsiConsole.MarkupLine(
                    $"Starting download of [u]Constellar.jar[/] ({progressTask.MaxValue} bytes)");

                await using Stream? contentStream = await response.Content.ReadAsStreamAsync();
                await using FileStream fileStream = new("Constellar.jar", FileMode.Create,
                    FileAccess.Write,
                    FileShare.None, bufferSize, true);


                byte[] buffer = new byte[bufferSize];

                while (true)
                {
                    if (contentStream == null)
                        continue;

                    int read = await contentStream.ReadAsync(buffer.AsMemory(0, buffer.Length));
                    if (read == 0)
                    {
                        AnsiConsole.MarkupLine("Download of [u]Constellar.jar[/] [green]completed![/]");
                        break;
                    }

                    // Increment the number of read bytes for the progress task
                    progressTask.Increment(read);

                    // Write the read bytes to the output stream
                    await fileStream.WriteAsync(buffer.AsMemory(0, read));
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error downloading gif:[/] {ex}");
            }
        }
    }
}