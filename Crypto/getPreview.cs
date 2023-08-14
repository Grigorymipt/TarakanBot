using System.Net.Http.Json;
using Serilog;

namespace Crypto;

//Retrieve the order information

public static class GetPreview
{
    static HttpClient httpClient = new HttpClient();

    class JsonApplication
    {
        public string? Id { get; set; }
    }

    static async Task getPreview(string Id, string? WpayStoreApiKey,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        JsonApplication jsonApplication = new JsonApplication()
        {
            Id = Id
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