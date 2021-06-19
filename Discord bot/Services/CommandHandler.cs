using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_bot.Services
{
    public class CommandHandler
    {
        public static IServiceProvider _provider;
        private static DiscordSocketClient _discord;
        private static CommandService _commands;
        private static IConfiguration _config;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider)
        {
            _provider = provider;
            _config = config;
            _commands = commands;
            _discord = discord;

            _discord.Ready += OnReady;
            _discord.MessageReceived += OnMessageReceived;

        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;

            if (msg.Author.IsBot) return;
            var context = new SocketCommandContext(_discord, msg);

            int pos = 0;
            if (msg.HasStringPrefix(_config["prefix"],ref pos) || msg.HasMentionPrefix(_discord.CurrentUser,ref pos))
            {
                var result = await _commands.ExecuteAsync(context, pos, _provider);

                if (!result.IsSuccess)
                {
                    var reason = result.Error;

                    await context.Channel.SendMessageAsync($"The following error occured: \n {reason}");
                    Console.WriteLine(reason);
                }
            }
        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }
    }
}
