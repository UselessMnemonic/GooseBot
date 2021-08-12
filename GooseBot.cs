using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GooseBot
{
    class GooseBot : BaseCommandModule
    {
        private static readonly string COMMAND_PREFIX = @"g!";
        private static readonly string HONK_PATTERN = @"h+j*[0oōóǒòôöõőøｏ]+(r+|n+|r+n+)k+";
        private static GooseBot instance = null;

        protected readonly DiscordClient client;
        protected readonly CommandsNextExtension commands;

        public static GooseBot GetInstance(string token, string[] replies)
        {
            if (instance == null)
            {
                instance = new GooseBot(token, replies);
            }
            return instance;
        }

        private GooseBot(string token, string[] replies)
        {
            GooseCommandModule.Replies.AddRange(replies);

            client = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
                MinimumLogLevel = LogLevel.Debug
            });
            commands = client.UseCommandsNext(new CommandsNextConfiguration()
            {
                EnableMentionPrefix = false,
                StringPrefixes = new[] { COMMAND_PREFIX }
            });

            client.MessageCreated += GooseCommandModule.DoHonk;
            commands.RegisterCommands<GooseCommandModule>();
        }

        public async Task RunAsync()
        {
            await client.ConnectAsync();
        }

        private class GooseCommandModule : BaseCommandModule
        {
            public static readonly List<string> Replies = new List<string>();

            public static async Task DoHonk(DiscordClient client, MessageCreateEventArgs e)
            {
                if (e.Author.Id != client.CurrentUser.Id)
                {
                    var msg = e.Message.Content.Trim().ToLower();
                    if (!msg.StartsWith(COMMAND_PREFIX) && Regex.IsMatch(msg, HONK_PATTERN))
                    {
                        var reply = Replies.GetRandomElement();
                        await e.Message.RespondAsync($"{reply} {e.Author.Mention}");
                    }
                }
            }

            [Command("bite")]
            [RequireGuild]
            public async Task BiteCommand(CommandContext ctx, [RemainingText] DiscordMember member)
            {
                if (member != null && member.Id != ctx.Client.CurrentUser.Id)
                {
                    var reply = Replies.GetRandomElement();
                    await ctx.RespondAsync($"{reply} {member.Mention}");
                }
            }
        }
    }
}
