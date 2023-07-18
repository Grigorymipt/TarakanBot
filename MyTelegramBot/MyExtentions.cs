using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.MyExtentions;

public class GetChannelMemberRequest : RequestBase<ChatMember>, IChatTargetable, IUserTargetable
{
    /// <inheritdoc />
    [JsonProperty(Required = Required.Always)]
    public ChatId ChatId { get; }

    /// <inheritdoc />
    [JsonProperty(Required = Required.Always)]
    public long UserId { get; }

    /// <summary>
    /// Initializes a new request with chatId and userId
    /// </summary>
    /// <param name="chatId">
    /// Unique identifier for the target chat or username of the target supergroup or channel
    /// (in the format <c>@channelusername</c>)
    /// </param>
    /// <param name="userId">Unique identifier of the target user</param>
    public GetChannelMemberRequest(ChatId chatId, long userId)
        : base("getChannelMember")
    {
        InputChannel = chatId;
        UserId = userId;
    }
}
public static class TelegramBotClientExtensions
{
    public static async Task<ChatMember> GetChatMemberAsync(
        this ITelegramBotClient botClient,
        ChatId chatId,
        long userId,
        CancellationToken cancellationToken = default
    ) =>
        await botClient.ThrowIfNull()
            .MakeRequestAsync(request: new GetChannelMemberRequest(chatId, userId), cancellationToken)
            .ConfigureAwait(false);
}
internal static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T ThrowIfNull<T>(
        this T? value,
        string? parameterName = default
    ) =>
        value ?? throw new ArgumentNullException(parameterName);
}