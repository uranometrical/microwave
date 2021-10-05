using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace Microwave.Utils
{
    public static class SpectreFxUtils
    {
        public static async Task AsyncProgressBar(string taskName, Func<ProgressTask, Task> task,
            bool autoStart = false)
        {
            await AnsiConsole.Console.Progress()
                .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(),
                    new RemainingTimeColumn(), new SpinnerColumn())
                .StartAsync(async ctx =>
                {
                    ProgressTask? progressTask = ctx.AddTask(taskName, new ProgressTaskSettings
                    {
                        AutoStart = autoStart
                    });

                    await task(progressTask);
                });
        }
    }
}