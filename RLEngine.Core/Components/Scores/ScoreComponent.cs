using RLEngine.Core;
using RLEngine.Core.Enumerations;
using System.Text.Json.Serialization;

namespace RLEngine.Core.Components.Scores
{
    public class ScoreComponent : GameComponent
    {

        [JsonInclude]
        public IDictionary<ScoreType, ScoreValue> ScoreSlots { get; set; } = new Dictionary<ScoreType, ScoreValue>();

        public ScoreComponent() : base()
        {
        }

        public ScoreComponent(Dictionary<ScoreType, ScoreValue> scoreSlots) : this()
        {
            ScoreSlots = scoreSlots;
        }

        public void AddScore(ScoreType scoreType, int defaultValue, int? defaultMaxValue = null)
        {
            if (!ScoreSlots.Any(x => x.Key == scoreType))
                ScoreSlots.Add(scoreType, new ScoreValue(defaultValue, defaultMaxValue ?? defaultValue));
        }

        public void AddScore(ScoreType scoreType, ScoreValue scoreValue)
        {
            if (!ScoreSlots.Any(x => x.Key == scoreType))
                ScoreSlots.Add(scoreType, scoreValue);
        }
        public void RemoveScore(ScoreType scoreType)
        {
            if (ScoreSlots.Any(x => x.Key == scoreType))
                ScoreSlots.Remove(scoreType);
        }

        protected ScoreValue GetScore(ScoreType scoreType)
        {
            return ScoreSlots.Where(x => x.Key == scoreType).FirstOrDefault().Value;
        }

        public int GetCurrentValue(ScoreType scoreType)
        {

            var scoreValue = GetScore(scoreType);
            var value = scoreValue?.Value ?? 0;

            var equippedItemsWithComponent = GameObject.GetComponent<EquipmentComponent>()?.GetEquippedItemsByComponent<ScoreModifierComponent>();
            if (equippedItemsWithComponent == null || scoreValue.ScoreComposition != ScoreComposition.Bonus) return value;
            var components = equippedItemsWithComponent.Select(x => x.GetComponent<ScoreModifierComponent>());
            return value + components?.Where(x => x.ScoreType == scoreType).Sum(x => x.ValueModifier) ?? 0;

        }

        public int GetCurrentMaxValue(ScoreType scoreType)
        {
            var scoreValue = GetScore(scoreType);
            var maxValue = scoreValue?.Value ?? 0;
            var equippedItemsWithComponent = GameObject.GetComponent<EquipmentComponent>()?.GetEquippedItemsByComponent<ScoreModifierComponent>();
            if (equippedItemsWithComponent == null) return maxValue;
            var components = equippedItemsWithComponent.Select(x => x.GetComponent<ScoreModifierComponent>());
            return maxValue + components?.Where(x => x.ScoreType == scoreType).Sum(x => x.ValueModifier) ?? 0;

        }

    }

}