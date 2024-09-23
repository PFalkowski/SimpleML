using Microsoft.ML.Data;

namespace SimpleML.NeuralNetworks.Models
{
    public class StockPricePrediction
    {
        [ColumnName("Score")]
        public float PredictedPrice { get; set; }
    }
}
