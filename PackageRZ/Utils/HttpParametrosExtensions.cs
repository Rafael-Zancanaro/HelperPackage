namespace PackageRZ.Utils;

public static class HttpParametrosExtensions
{
    public static async Task<string> ObterParametrosAsync(this HttpRequestMessage requestMessage)
    {
        if (requestMessage is null)
            return null;

        var query = requestMessage.RequestUri?.Query;
        if (!string.IsNullOrEmpty(query))
            return query;

        return await ObterPayloadAsync(requestMessage);
    }

    private static async Task<string> ObterPayloadAsync(HttpRequestMessage requestMessage)
    {
        if (requestMessage?.Content is null)
            return null;

        try
        {
            return await requestMessage.Content.ReadAsStringAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }
}
