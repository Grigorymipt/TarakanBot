namespace Crypto;

public class TransactionInfo
{
    private static readonly HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://blockchain.info/rawtx/")
    };
    public static async Task<string> GetTransaction(string transactionHash)
    {
        using (HttpResponseMessage responseMessage = await sharedClient.GetAsync(transactionHash))
        {
            responseMessage.EnsureSuccessStatusCode();
            var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
            return jsonResponse;
        }
    }
}