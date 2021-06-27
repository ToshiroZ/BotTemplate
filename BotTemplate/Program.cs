using System;
using System.Threading.Tasks;
using BotTemplate;

namespace BotTemplate
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, eventArgs) => Console.WriteLine(eventArgs.ExceptionObject);
            await Startup.RunAsync(args);
        }
    }
}