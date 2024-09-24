namespace SimpleML.AlphaVintageClient;

public interface IStockDataProvider
{
    Task<List<StockData>> GetData();
}