using System.Net.Http.Json;

namespace Crypto;

public static class WalletInfo
{
    static HttpClient httpClient = new HttpClient();

    //classes for creating JsonApplication
    class Amount
    {
        public string currencyCode { get; set; }
        public string amount { get; set; }
    }

    class JsonApplication
    {
        public Amount amount { get; set; }
        public string description { get; set; }
        public string? returnUrl { get; set; }
        public string? failReturnUrl { get; set; }
        public string? customData { get; set; }
        public string externalId { get; set; }
        public int timeoutSeconds { get; set; }
        public int customerTelegramUserId { get; set; }
    }

    //PostRequest for getting Status Code
    //Example PostAsync:
    //"USD", "1.00", "VPN for 1 month",
    //"https://t.me/wallet", "https://t.me/wallet",
    //"client_ref=4E89", "ORD-5023-4E89",
    //1800, 0, Key API

    static async Task PostAsync(
        string currencyCode,
        string amount,
        string description,
        string? returnUrl,
        string? failReturnUrl,
        string? customData,
        string externalId,
        int timeoutSeconds,
        int customerTelegramUserId,
        string? WpayStoreApiKey,
        CancellationToken cancellationToken = default(CancellationToken)
        )
    {
        JsonApplication jsonApplication = new JsonApplication()
        {
            amount = new Amount()
            {
                currencyCode = currencyCode,
                amount = amount
            },
            description = description,
            returnUrl = returnUrl,
            failReturnUrl = failReturnUrl,
            customData = customData,
            externalId = externalId,
            timeoutSeconds = timeoutSeconds,
            customerTelegramUserId = customerTelegramUserId,
        };

        // creating JsonContent
        JsonContent content = JsonContent.Create(jsonApplication);

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://pay.wallet.tg/wpay/store-api/v1/order");
        request.Content = content;

        request.Headers.Add("Wpay-Store-Api-Key", WpayStoreApiKey);
        request.Headers.Add("Content-Type", "application/json");
        request.Headers.Add("Accept", "application/json");

        using (HttpResponseMessage responseMessage = await httpClient.SendAsync(request))
        {
            responseMessage.EnsureSuccessStatusCode();
            var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(responseMessage.EnsureSuccessStatusCode());
        }

    }
}