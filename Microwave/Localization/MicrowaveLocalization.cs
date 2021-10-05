using System.Collections.Generic;
using Spectre.Console;
using TomatoKnishes.Localization;
using TomatoKnishes.Localization.Implementation;

namespace Microwave.Localization
{
    public class MicrowaveLocalization : LocalizationProvider<LocalizationType>
    {
        public override IDictionary<LocalizationType, ILocalizedTextEntry> TextEntries { get; } =
            new Dictionary<LocalizationType, ILocalizedTextEntry>
            {
                {
                    LocalizationType.StaticText, new LocalizedTextEntry(
                        (LocalizationConstants.English, $"\nWelcome to [bold]Microwave[/], [#{Color.SlateBlue1.ToHex()}]Constellar[/]'s set-up tool." +
                                                        $"\nRunning version {typeof(MicrowaveLocalization).Assembly.GetName().Version}" +
                                                        //$"\n  Headless: {Program.Headless}" +
                                                        //$"\n  DownloadFile Profile: {Program.Profile}" +
                                                        $"\n")
                    )
                },

                {
                    LocalizationType.InstallationSetUp, new LocalizedTextEntry(
                        (LocalizationConstants.English, "Installation set-up:")
                    )
                },

                {
                    LocalizationType.DownloadingConstellar, new LocalizedTextEntry(
                        (LocalizationConstants.English, "Downloading Constellar.jar..."))
                }
            };
    }
}