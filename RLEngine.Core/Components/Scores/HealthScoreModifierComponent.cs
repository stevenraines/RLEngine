using RLEngine.Core;
using RLEngine.Core.Events;

namespace RLEngine.Core.Components
{
    public class HealthScoreModifierComponent : GameComponent
    {

        public event EventHandler MaxValueModifierChanged;

        public int MaxValueModifier { get; set; }

        public HealthScoreModifierComponent() { }

        public HealthScoreModifierComponent(int maxValueModifier) : base()
        {
            MaxValueModifier = maxValueModifier;
        }

        public void SetMaxValueModifier(int maxValueModifier)
        {
            if (MaxValueModifier == maxValueModifier) return;
            MaxValueModifier = maxValueModifier;
            MaxValueModifierChanged.Invoke(this, new ValueChangedEventArgs() { Value = MaxValueModifier });
        }

    }

}