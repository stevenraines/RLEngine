using RLEngine.Core;
using RLEngine.Core.Events;

namespace RLEngine.Core.Components
{
    public class HealthScoreComponent : GameComponent, IScoreComponent
    {

        public event EventHandler ValueChanged;
        public event EventHandler MaxValueChanged;
        public int Value { get; set; }
        public int MaxValue { get; set; }

        public int ModifiedMaxValue { get { return GetModifiedMaxValue(); } }

        public HealthScoreComponent() { }

        public HealthScoreComponent(int value, int maxValue) : base()
        {
            Value = value;
            MaxValue = maxValue;
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

        public int GetModifiedMaxValue()
        {

            var equippedItemsWithComponent = GameObject.GetComponent<EquipmentComponent>()?.GetEquippedItemsByComponent<HealthScoreModifierComponent>();
            if (equippedItemsWithComponent == null) return MaxValue;
            return MaxValue + equippedItemsWithComponent.Select(x => x.GetComponent<HealthScoreModifierComponent>()).Sum(x => x.MaxValueModifier);

        }

    }

}