using TL;

namespace MyTelegramBot.Types;

public static class CryproLink
{
    public static string Get(CryproLinkParams cryproLinkParams)
    {
        var vendor = Environment.GetEnvironmentVariable("PaymendVendor");
        string link = vendor switch
        {
            "wallet" => Crypto.CreateOrder.PostAsync(
                currencyCode: cryproLinkParams.currencyCode,
                amount: cryproLinkParams.amount,
                description: cryproLinkParams.description,
                customData: cryproLinkParams.customData,
                externalId: cryproLinkParams.externalId,
                timeoutSeconds: (int)cryproLinkParams.timeoutSeconds,
                customerTelegramUserId: (int)cryproLinkParams.customerTelegramUserId,
                WpayStoreApiKey: cryproLinkParams.WpayStoreApiKey).Result,
            _ => throw new NullReferenceException($"Payment vendor {vendor} not supported")
        };
        return link;
    }   
}
public record CryproLinkParams(
    string currencyCode,
    string amount,
    string description,
    string? customData = null,
    string? externalId = null,
    int? timeoutSeconds = null,
    int? customerTelegramUserId = null,
    string? WpayStoreApiKey = null
);