using Microsoft.ML.Data;

namespace SimpleML.NeuralNetworks.Models
{
    public class StockPrice
    {
        [LoadColumn(0)]
        public float DayIndex { get; set; }

        [LoadColumn(1)]
        public float Price { get; set; }
    }
}
