using System;
using System.Threading.Tasks;
using BotTemplate.CustomContexts;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace BotTemplate.Services;

public class InteractionHandler
{
    private readonly DiscordShardedClient _client;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _provider;

    public InteractionHandler(
        DiscordShardedClient client,
        InteractionService interactionService,
        IConfigurationRoot config,
        IServiceProvider provider)
    {
        _client = client;
        _interactionService = interactionService;
        _provider = provider;

        _client.InteractionCreated += ClientOnInteractionCreated;
    }
    private async Task ClientOnInteractionCreated(SocketInteraction arg)
    {
        var context = new CustomShardedInteractionContext(_client, arg);
        var result = await _interactionService.ExecuteCommandAsync(context, _provider);
        if (!result.IsSuccess)
        {
            Console.WriteLine(result.ErrorReason);
        }
    }
}