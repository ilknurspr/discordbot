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

        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }
    }
}
