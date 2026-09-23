namespace PackageRZ.Utils;

/// <summary>
/// Provides extension methods for HttpRequestMessage to extract parameters.
/// </summary>
public static class HttpParametersExtensions
{
    /// <summary>
    /// Asynchronously retrieves the parameters from the HTTP request message.
    /// It first attempts to return the query string; if empty, it attempts to return the request payload.
    /// </summary>
    /// <param name="requestMessage">The HTTP request message.</param>
    /// <returns>A task returning the parameters as a string, or null if none are found.</returns>
    public static async Task<string> GetParametersAsync(this HttpRequestMessage requestMessage)
    {
        if (requestMessage is null)
            return null;

        var query = requestMessage.RequestUri?.Query;
        if (!string.IsNullOrEmpty(query))
            return query;

        return await GetPayloadAsync(requestMessage);
    }

    private static async Task<string> GetPayloadAsync(HttpRequestMessage requestMessage)
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
