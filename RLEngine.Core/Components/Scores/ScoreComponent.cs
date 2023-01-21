using RLEngine.Core;
using RLEngine.Core.Events;
using RLEngine.Core.Components.Scores;

namespace RLEngine.Core.Components
{
    public class ScoreComponent : GameComponent, IScoreComponent
    {

        public ScoreType ScoreType { get; set; }

        public event EventHandler ValueChanged;
        public event EventHandler MaxValueChanged;


        // theunderlying current value;
        public int Value { get; set; }

        // the maximum the current value can be set-to
        public int MaxValue { get; set; }

        // the value with modifiers applied
        public int CurrentValue { get { return GetCurrentValue(); } }

        // the max value as currently modified
        public int CurrentMaxValue { get { return GetCurrentMaxValue(); } }

        public ScoreComponent(ScoreType scoreType) : base()
        {
            ScoreType = scoreType;
        }

        public ScoreComponent(ScoreType scoreType, int value, int? maxValue = null) : this(scoreType)
        {

            Value = value;
            MaxValue = maxValue ?? value;
        }

        public void SetValue(int value)
        {
            if (Value == value) return;
            Value = value;
            ValueChanged.Invoke(this, new ValueChangedEventArgs() { Value = Value });
        }

        public void SetMaxValue(int maxValue)
        {
            if (MaxValue == maxValue) return;
            MaxValue = maxValue;
            MaxValueChanged.Invoke(this, new ValueChangedEventArgs() { Value = MaxValue });
        }

        public int GetCurrentValue()
        {
            var equippedItemsWithComponent = GameObject.GetComponent<EquipmentComponent>()?.GetEquippedItemsByComponent<ScoreModifierComponent>();
            if (equippedItemsWithComponent == null) return Value;
            var components = equippedItemsWithComponent.Select(x => x.GetComponent<ScoreModifierComponent>());

            var bonusComponents = components?.Where(x => x.ScoreType.ScoreComposition == ScoreComposition.Bonus);
            return Value + bonusComponents.Sum(x => x.ValueModifier);
        }
        public int GetCurrentMaxValue()
        {

            var equippedItemsWithComponent = GameObject.GetComponent<EquipmentComponent>()?.GetEquippedItemsByComponent<ScoreModifierComponent>();
            if (equippedItemsWithComponent == null) return MaxValue;
            return MaxValue + equippedItemsWithComponent.Select(x => x.GetComponent<ScoreModifierComponent>()).Sum(x => x.ValueModifier);

        }

    }

}