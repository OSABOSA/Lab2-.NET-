// See https://aka.ms/new-console-template for more information
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;

string apiKey = "9f0de56e2457422e8e5cebb93e5f8426";
string baseUrl = "https://openexchangerates.org/api/latest.json";

// Define the parameters for the API request
var queryParams = new System.Collections.Generic.Dictionary<string, string>
{
    { "app_id", apiKey },
    { "base", "USD" },  // Base currency
    { "symbols", "EUR,GBP,JPY,KES" }  // Target currencies
};

// Construct the query string
string queryString = string.Join("&", queryParams
    .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));


// Construct the full URL
string url = $"{baseUrl}?{queryString}";

using (HttpClient client = new HttpClient())
{
    try{
        // Make the API request
        HttpResponseMessage response = await client.GetAsync(url);

        // Check if the request was successful
        if (response.IsSuccessStatusCode){
            string responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(responseBody);
            var rates = data.rates;
            Console.WriteLine(data.ToString());
        }
        else{
            Console.WriteLine($"Error occurred while fetching exchange rates. Status code: {response.StatusCode}");
        }
    }
    catch (HttpRequestException e){
        Console.WriteLine($"An error occurred: {e.Message}");
    }
}
public class ExchangeRateResponse
{
    public string @base { get; set; }
    public DateTime time { get; set; }
    public System.Collections.Generic.Dictionary<string, decimal> rates { get; set; }

    public ExchangeRateResponse() { }

    public override string ToString()
    {
        string s_rates = "";
        for (int i = 0;
            i < rates.Count;
            i++
            )
        {
            s_rates += $"{rates.ElementAt(i).Key}: {rates.ElementAt(i).Value}\n";
        }
        return $"Base: {@base}, Time: {time}, Rates: \n" + s_rates;
    }
}