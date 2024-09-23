﻿using System.Globalization;
using Newtonsoft.Json.Linq;

namespace SimpleML.AlphaVintageClient;

public class StockDataFetcher
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public StockDataFetcher(string apiKey)
    {
        _httpClient = new HttpClient();
        _apiKey = apiKey;
    }

    public async Task<List<StockData>> FetchStockDataAsync(string symbol, int limit = 1000)
    {
        string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={_apiKey}&outputsize=full";
        var response = await _httpClient.GetStringAsync(url);
        var jobject = JObject.Parse(response);

        var timeSeries = jobject["Time Series (Daily)"].ToObject<Dictionary<string, JObject>>();
        return timeSeries
            .Select(kvp => new StockData
            {
                Date = DateTime.Parse(kvp.Key, CultureInfo.InvariantCulture),
                Close = double.Parse(kvp.Value["4. close"].ToString(), CultureInfo.InvariantCulture)
            })
            .OrderByDescending(sd => sd.Date)
            .Take(limit)
            .Reverse()
            .ToList();
    }
}