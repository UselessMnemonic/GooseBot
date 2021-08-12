using System;
using System.IO;
using System.Threading.Tasks;

namespace GooseBot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: GooseBot <env> <replies>");
            }
            else if (args.Length < 2)
            {
                Console.WriteLine("Missing <replies>");
            }
            else
            {
                MainAsync(args[0], args[1]).GetAwaiter().GetResult();
            }
        }

        static async Task MainAsync(string envPath, string repliesPath)
        {
            var env = await File.ReadAllLinesAsync(envPath);
            var replies = await File.ReadAllLinesAsync(repliesPath);
            var bot = GooseBot.GetInstance(env[0], replies);
            await bot.RunAsync();
            await Task.Delay(-1);
        }
    }
}
