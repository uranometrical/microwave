using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace Microwave.Utils
{
    public static class SpectreFxUtils
    {
        public static async Task AsyncProgressBar(Func<ProgressContext, Task> task)
        {
            await AnsiConsole.Console.Progress()
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new RemainingTimeColumn(),
                    new SpinnerColumn()
                )
                .StartAsync(async ctx =>
                {
                    await task(ctx);
                });
        }
    }
}