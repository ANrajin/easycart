using Nop.Services.ScheduleTasks;

namespace Nop.Plugin.Widgets.HelloWorld.ScheduleTasks;
public class SimpleScheduleTask : IScheduleTask
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("Simple schedule task executed...");
        return Task.CompletedTask;
    }
}
