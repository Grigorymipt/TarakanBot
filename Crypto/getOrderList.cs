using System.Collections.Generic;
using System.Net.Http.Json;
using Serilog;

namespace Crypto;

//Return list of store orders sorted by creation time in ascending order

public static class GetOrderList
{
    static HttpClient httpClient = new HttpClient();

    class JsonApplication
    {
        //Specifying the amount of excluded from a response the first N orders
        public int? offset { get; set; }
        //Specifying the limit of orders for the request
        public int? count { get; set; }
    }

    static async Task getOrderList(int offset, int count, string? WpayStoreApiKey,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        JsonApplication jsonApplication = new JsonApplication()
        {
            offset = offset,
            count = count
        };

        // creating JsonContent
        JsonContent content = JsonContent.Create(jsonApplication);

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://pay.wallet.tg/wpay/store-api/v1/order");
        request.Content = content;

        request.Headers.Add("Wpay-Store-Api-Key", WpayStoreApiKey);

        using (HttpResponseMessage responseMessage = await httpClient.SendAsync(request))
        {
            responseMessage.EnsureSuccessStatusCode();
            var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(responseMessage.EnsureSuccessStatusCode());
            Log.Error(responseMessage.EnsureSuccessStatusCode().ToString());
        }

    }
}