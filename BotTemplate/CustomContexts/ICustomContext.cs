using System.Threading.Tasks;
using Discord;

namespace BotTemplate.CustomContexts
{
    public interface ICustomContext : IInteractionContext
    {
        Task<IUserMessage> OkAsync(string message, bool ephemeral = false); 
        Task<IUserMessage> ErrorAsync(string message, bool ephemeral = false);
    }
}