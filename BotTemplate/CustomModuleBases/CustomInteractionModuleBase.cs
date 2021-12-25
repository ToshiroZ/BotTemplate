using System.Threading.Tasks;
using BotTemplate.CustomContexts;
using Discord;
using Discord.Interactions;

namespace BotTemplate.CustomModuleBases
{
    public class CustomInteractionModuleBase<T> : InteractionModuleBase<T> where T: class, ICustomContext
    {
        public async Task<IUserMessage> OkAsync(string message, bool ephemeral = false)
        {
            return await base.Context.OkAsync(message, ephemeral);
        }
        public async Task<IUserMessage> ErrorAsync(string message, bool ephemeral = false)
        {
            return await base.Context.ErrorAsync(message, ephemeral);
        }
    }
}