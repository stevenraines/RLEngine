using System.Text.Json.Serialization;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core.Components.Scores
{
    public class ScoreValue
    {

        [JsonInclude]
        public int Value { get; set; }

        [JsonInclude]
        public int MaxValue { get; set; }

        [JsonInclude]
        public ScoreComposition ScoreComposition { get; set; }

        public ScoreValue() { }

        public ScoreValue(int value, int? maxValue = null, ScoreComposition scoreComposition = ScoreComposition.Bonus)
        {
            Value = value;
            MaxValue = maxValue ?? Value;
            ScoreComposition = scoreComposition;
        }
    }
}