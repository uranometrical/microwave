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
<<<<<<< HEAD
                        (LocalizationConstants.English,
                            $"\nWelcome to [bold]Microwave[/], [#{Color.SlateBlue1.ToHex()}]Constellar[/]'s set-up tool." +
                            $"\nRunning version {typeof(MicrowaveLocalization).Assembly.GetName().Version}" +
                            //$"\n  Headless: {Program.Headless}" +
                            //$"\n  Download Profile: {Program.Profile}" +
                            "\n")
=======
                        (LocalizationConstants.English, $"\nWelcome to [bold]Microwave[/], [#{Color.SlateBlue1.ToHex()}]Constellar[/]'s set-up tool." +
                                                        $"\nRunning version {typeof(MicrowaveLocalization).Assembly.GetName().Version}" +
                                                        //$"\n  Headless: {Program.Headless}" +
                                                        //$"\n  DownloadFile Profile: {Program.Profile}" +
                                                        $"\n")
>>>>>>> 38cc55c566574fb6a0a1d81768dfe7b2933cc6fe
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