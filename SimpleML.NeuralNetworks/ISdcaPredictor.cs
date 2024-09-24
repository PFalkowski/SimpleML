using SimpleML.AlphaVintageClient;

namespace SimpleML.NeuralNetworks;

public interface ISdcaPredictor
{
    void TrainModel(List<StockData> stockData);
    float PredictPrice(float dayIndex);
    void EvaluateModel(List<StockData> testData);
}