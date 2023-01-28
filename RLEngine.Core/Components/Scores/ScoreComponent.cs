using RLEngine.Core;
using RLEngine.Core.Enumerations;
using System.Text.Json.Serialization;

namespace RLEngine.Core.Components.Scores
{
    public class ScoreComponent : GameComponent
    {

        [JsonInclude]
        public IDictionary<ScoreType, ScoreValue> ScoreSlots { get; set; } = new Dictionary<ScoreType, ScoreValue>();


        public ScoreComponent()
        {
        }
        public ScoreComponent(Dictionary<ScoreType, ScoreValue> scoreSlots) : base()
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

        private int GetValueModifierForType(ScoreType scoreType)
        {
            var scoreValue = GetScore(scoreType);
            var equippedItemsWithComponent = GameObject.GetComponent<EquipmentComponent>()?.GetEquippedItemsByComponent<ScoreModifierComponent>();
            if (equippedItemsWithComponent == null || scoreValue.ScoreComposition != ScoreComposition.Bonus) return 0;
            var scoreModifierComponents = equippedItemsWithComponent.Select(x => x.GetComponent<ScoreModifierComponent>());
            var scoreModifierComponentsForType = scoreModifierComponents.Where(x => x.ScoreType == scoreType).ToList();
            return scoreModifierComponentsForType?.Sum(x => x.ValueModifier) ?? 0;

        }

        public int GetCurrentValue(ScoreType scoreType)
        {

            var scoreValue = GetScore(scoreType);
            var value = scoreValue?.Value ?? 0;
            return value + GetValueModifierForType(scoreType);

        }

        public int GetCurrentMaxValue(ScoreType scoreType)
        {
            var scoreValue = GetScore(scoreType);
            var maxValue = scoreValue?.Value ?? 0;
            return maxValue + GetValueModifierForType(scoreType);
        }

    }

}