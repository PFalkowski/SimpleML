namespace SimpleML.AlphaVintageClient;

public interface IStockDataFetcher
{
    Task<List<StockData>> FetchStockDataAsync(string symbol, int limit = 1000);
}