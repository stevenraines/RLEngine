using RLEngine.Core;
using RLEngine.Core.Events;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core.Components.Scores
{


    public class ScoreModifierComponent : GameComponent
    {

        public ScoreType ScoreType { get; set; }

        public event EventHandler ValueModifierChanged;
        public int ValueModifier { get; set; }

        public ScoreModifierComponent(ScoreType scoreType) : base()
        {
            ScoreType = ScoreType;
        }

        public ScoreModifierComponent(ScoreType scoreType, int valueModifier) : this(scoreType)
        {
            ValueModifier = valueModifier;
        }

        public void SetMaxValueModifier(int valueModifier)
        {
            if (ValueModifier == valueModifier) return;
            ValueModifier = valueModifier;
            ValueModifierChanged.Invoke(this, new ValueChangedEventArgs() { Value = ValueModifier });
        }

    }

}