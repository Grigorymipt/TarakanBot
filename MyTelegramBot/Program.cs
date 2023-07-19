using System;

namespace MyTelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Config.BotToken);
            Bot bot = new Bot()
            {
                Token = Config.BotToken,
                
            };
            
            bot.Init().Wait();
        }
    }
}