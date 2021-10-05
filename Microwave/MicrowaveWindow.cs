using System;
using System.Collections.Generic;
using CliFx;
using Microwave.Commands;
using Microwave.Localization;
using Spectre.Console;
using TomatoKnishes.Internals;
using TomatoKnishes.SpectreFx;

namespace Microwave
{
    public class MicrowaveWindow : ConsoleWindow
    {
        public override CommandContainer DefaultCommandSet => CreateContainer(new List<ICommand>
        {
            new StableInstallationCommand(),
            new BleedingEdgeInstallationCommand()
        }, Knishes.GetLocalizedText(LocalizationType.InstallationSetUp), false, false);

        public override void WriteStaticConsole(Action? preWrite = null, Action? postWrite = null)
        {
            preWrite?.Invoke();

            AnsiConsole.MarkupLine(Knishes.GetLocalizedText(LocalizationType.StaticText));

            postWrite?.Invoke();
        }
    }
}