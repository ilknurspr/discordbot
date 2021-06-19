using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Discord_bot.Services
{
    public class StartupService
    {
        public static IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;

        public StartupService(IServiceProvider provider,DiscordSocketClient discord,CommandService commands,IConfigurationRoot config)
        {
            _provider = provider;
            _config = config;
            _commands = commands;
            _discord = discord;
        }

        public async Task StartAsync()
        {
            string token = _config["tokens:discord"];
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Please provide your Discord token in _config.yml");
                return;
            }

            await _discord.LoginAsync(TokenType.Bot, token);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}
