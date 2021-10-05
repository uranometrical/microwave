using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using TomatoKnishes.SpectreFx.Identities;

namespace Microwave.Commands
{
    [Command("bleedingedge")]
    public class BleedingEdgeInstallationCommand : ICommand, ICommandIdentity
    {
        public string CommandName => "Bleeding Edge Installation";
        public string CommandDescription => "Installs the latest build of Constellar fresh from GitHub.";

        public ValueTask ExecuteAsync(IConsole console)
        {
            throw new System.NotImplementedException();
        }
    }
}