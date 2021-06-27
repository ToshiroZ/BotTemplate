using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace BotTemplate.Services
{
    internal class LoggingHandler
    {
        public LoggingHandler(DiscordShardedClient discord, CommandService commands)
        {
            discord.Log += Log;
            commands.Log += Log;
        }

        private static Task Log(LogMessage message)
        {
            Console.ForegroundColor = message switch
            {
                {Severity: LogSeverity.Critical} => ConsoleColor.Red,
                {Severity: LogSeverity.Error} => ConsoleColor.Red,
                {Severity: LogSeverity.Warning} => ConsoleColor.Yellow,
                {Severity: LogSeverity.Info} => ConsoleColor.White,
                {Severity: LogSeverity.Verbose} => ConsoleColor.Gray,
                {Severity: LogSeverity.Debug} => ConsoleColor.Gray,
                _ => ConsoleColor.Gray
            };
            Console.WriteLine($"{DateTime.Now:h:mm:ss tt} [{message.Severity}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();
            
            return Task.CompletedTask;
        }
    }
}