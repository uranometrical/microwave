using System;
using CliFx;
using Microwave.Localization;
using Spectre.Console;
using TomatoKnishes.Internals;
using TomatoKnishes.SpectreFx;

namespace Microwave
{
    public class MicrowaveWindow : ConsoleWindow
    {
        public override CommandContainer DefaultCommandSet => CreateContainer(new ICommand[]
            {

            },
            Knishes.GetLocalizedText(LocalizationType.InstallationSetUp), false, false);

        public override void WriteStaticConsole(Action? preWrite = null, Action? postWrite = null)
        {
            preWrite?.Invoke();

            AnsiConsole.MarkupLine(Knishes.GetLocalizedText(LocalizationType.StaticText));

            postWrite?.Invoke();
        }
    }
}