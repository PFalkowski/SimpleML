using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace SimpleML.AlphaVintageClient
{
    public class StockDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _filePath;
        private readonly string _apiKey;

        public StockDataProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _filePath = configuration["DataStorage:StockDataFilePath"];
            if (string.IsNullOrEmpty(_filePath))
            {
                throw new ArgumentException("Stock data file path not found in configuration.");
            }
            _apiKey = _configuration["ApiSettings:AlphaVantageApiKey"];
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API key not found in configuration.");
            }
        }

        public async Task<List<StockData>> GetData()
        {
            List<StockData> stockData;

            if (DataFileExists())
            {
                Console.WriteLine("Loading stock data from file...");
                stockData = await LoadDataAsync();
            }
            else
            {
                Console.WriteLine("Fetching stock data from API...");

                var stockDataFetcher = new StockDataFetcher(_apiKey);
                stockData = await stockDataFetcher.FetchStockDataAsync("MSFT", 1000);

                Console.WriteLine("Saving stock data to file...");
                await SaveDataAsync(stockData);
            }

            return stockData;
        }

        public async Task SaveDataAsync(List<StockData> data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(_filePath, jsonString);
        }

        public async Task<List<StockData>> LoadDataAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<StockData>();
            }

            var jsonString = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<StockData>>(jsonString) ?? new List<StockData>();
        }

        public bool DataFileExists()
        {
            return File.Exists(_filePath);
        }
    }
}
