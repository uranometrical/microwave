using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace Microwave.Utils
{
    public static class SpectreFxUtils
    {
<<<<<<< HEAD
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
=======
        public static async Task AsyncProgressBar(
            string taskName,
            Func<ProgressTask, Task> task,
            bool autoStart = false
        )
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
                    ProgressTask progressTask = ctx.AddTask(
                        taskName,
                        new ProgressTaskSettings {AutoStart = autoStart}
                    );
>>>>>>> 38cc55c566574fb6a0a1d81768dfe7b2933cc6fe

                    await task(progressTask);
                });
        }
    }
}