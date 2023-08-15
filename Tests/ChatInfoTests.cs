using System;
using System.Diagnostics;
using System.Linq;
using Bridge;
using MyTelegramBot;
using MyTelegramBot.Types;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using TL;
using Xunit;
using MyTelegramBot.Listeners;
namespace Tests;


public class ChatInfoTests
{
    [Fact]
    public async void TestChatIf()
    {
        var channelId = -1001927535266;
        var userId = 710549873;

        TelegramBotClient botClient = new TelegramBotClient(MyTelegramBot.Config.BotToken);
        // Console.WriteLine(await ChannelInfo.MemberStatusChat(botClient, channelId, userId));
    }
}