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
    [Command("stable")]
    public class StableInstallationCommand : ICommand, ICommandIdentity
    {
        public const string LatestReleaseUrl =
            "https://api.github.com/repos/Uranometrical/Constellar/releases/latest";
        public string CommandName => "Stable Installation";
        public string CommandDescription => "Installs the latest stable release of Constellar.";

        public async ValueTask ExecuteAsync(IConsole console)
        {
            // todo: output path and minecraft/multimc profiles
            
            AnsiConsole.WriteLine("Installing stable release");
            
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Constellar/v0.1.0-alpha");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            var releaseResponse = await httpClient.GetAsync(LatestReleaseUrl);
            var releaseResponseJson = await releaseResponse.Content.ReadAsStringAsync();

            var latestRelease = JsonConvert.DeserializeObject<RepositoryRelease>(releaseResponseJson);

            foreach(var asset in latestRelease.Assets)
                AnsiConsole.WriteLine(asset.Name + " " + asset.BrowserDownloadUrl);
            
            // todo: version independent
            // find the download url of the first asset with the name of "ConstellarMain-0.1.0-alpha.jar"
            var constellarUrl = latestRelease.Assets.Find(asset => asset.Name == "ConstellarMain-0.1.0-alpha.jar").BrowserDownloadUrl;

            AnsiConsole.WriteLine(constellarUrl);
            
            if (constellarUrl == string.Empty)
            {
                AnsiConsole.WriteLine("Release not found!");
            }
            
            await AnsiConsole.Console.Progress()
                .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(), new SpinnerColumn())
                .StartAsync(async ctx =>
                {
                    var progressTask = ctx.AddTask(Knishes.GetLocalizedText(LocalizationType.DownloadingConstellar), new ProgressTaskSettings
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
                        var client = new HttpClient();
                        using var response = await client.GetAsync(constellarUrl,
                            HttpCompletionOption.ResponseHeadersRead);
                        response.EnsureSuccessStatusCode();

                        // Set the max value of the progress task to the number of bytes
                        progressTask.MaxValue(response.Content.Headers.ContentLength ?? 0);
                        // Start the progress task
                        progressTask.StartTask();

                        //todo: localization
                        AnsiConsole.MarkupLine($"Starting download of [u]Constellar.jar[/] ({progressTask.MaxValue} bytes)");

                        await using var contentStream = await response.Content.ReadAsStreamAsync();
                        await using var fileStream = new FileStream("Constellar.jar", FileMode.Create,
                            FileAccess.Write,
                            FileShare.None, bufferSize, true);


                        var buffer = new byte[bufferSize];

                        while (true)
                        {
                            if (contentStream == null)
                                continue;

                            var read = await contentStream.ReadAsync(buffer.AsMemory(0, buffer.Length));
                            if (read == 0)
                            {
                                AnsiConsole.MarkupLine($"Download of [u]Constellar.jar[/] [green]completed![/]");
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
                });
            
            AnsiConsole.WriteLine("Download complete!");
        }
    }
}