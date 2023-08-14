using static Crypto.CreateOrder;
using System.Net.Http.Json;
using Serilog;
using System.Text.Json;

namespace Crypto;

public static class CreateOrder
{
    static HttpClient httpClient = new HttpClient();

    //classes for creating JsonApplication
    public class Amount
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

    public static async Task<string> PostAsync(
        string currencyCode,
        string amount,
        string description,
        string externalId,
        int timeoutSeconds,
        int customerTelegramUserId,
        string? returnUrl = null,
        string? failReturnUrl = null,
        string? customData = null,
        string? WpayStoreApiKey = null,
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
        Log.Information("Sending payment link request via wallet api");
        using (HttpResponseMessage responseMessage = await httpClient.SendAsync(request))
        {
            Log.Information("Response received");
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                // TODO: Grisha adds db save eventId to user
                Log.Information("smth");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

           
            var jsonResponse = await responseMessage.Content.ReadAsStringAsync();

            ResponseCreate? responseCreate =
                JsonSerializer.Deserialize<ResponseCreate>(jsonResponse);
            return responseCreate.data.payLink;
        }
    }
    //ResponseCreate for CreateOrder
    public class Data
    {
        public string id { get; set; }
        public string status { get; set; }
        public string number { get; set; }
        public Amount amount { get; set; }
        public string createdDateTime { get; set; }
        public string expirationDateTime { get; set; }
        public string? completedDateTime { get; set; }
        public string payLink { get; set; }
        public string directPayLink { get; set; }
    }

    public class ResponseCreate
    {
        public string status { get; set; }
        public string? message { get; set; }
        public Data data { get; set; }
    }
    //ResponseWebHook for CryptoController
    public class PaymentOption
    {
        public Amount amount { get; set; }
        public Amount amountFee { get; set; }
        public Amount amountNet { get; set; }
        public string exchangeRate { get; set; }
    }

    public class Payload
    {
        public int id { get; set; }
        public string number { get; set; }
        public string externalId { get; set; }
        public string? status { get; set; }
        public string? customData { get; set; }
        public Amount orderAmount { get; set; }
        public PaymentOption? selectedPaymentOption { get; set; }
        public string orderCompletedDateTime { get; set; }       
    }

    public class ResponseWebHook
    {
        public string eventDateTime { get; set; }
        public int eventId { get; set; }
        public string type { get; set; }
        public Payload payload { get; set; }
    }
}