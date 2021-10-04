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
                        (LocalizationConstants.English, $"\n  Welcome to [invert]Microwave[/], [#{Color.Purple.ToHex()}]Constellar[/]'s set-up tool." +
                                                        $"\n  Running version {typeof(MicrowaveLocalization).Assembly.GetName().Version}" +
                                                        $"\n  Headless: {Program.Headless}" +
                                                        $"\n  Download Profile: {Program.Profile}" +
                                                        $"\n")
                    )
                },

                {
                    LocalizationType.InstallationSetUp, new LocalizedTextEntry(
                        (LocalizationConstants.English, "Installation set-up:")
                    )
                }
            };
    }
}