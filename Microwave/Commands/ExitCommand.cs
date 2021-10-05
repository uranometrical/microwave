using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using TomatoKnishes.SpectreFx.Identities;

namespace Microwave.Commands
{
    [Command("exit", Description = "Exits the installer. This is for the CLI only.")]
    public class ExitCommand : ICommand, ICommandIdentity
    {
        public string CommandName => "Exit";

        public string CommandDescription => "Exits the installer.";

        public async ValueTask ExecuteAsync(IConsole console)
        {
            if (Program.Headless)
                throw new Exception("Do not use this while not using the CLI installer.");

            Environment.Exit(0);
            await Task.CompletedTask;
        }
    }
}