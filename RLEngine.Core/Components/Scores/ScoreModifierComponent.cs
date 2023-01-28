using System.Text.Json.Serialization;
using RLEngine.Core.Events;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core.Components.Scores
{

    public class ScoreModifierComponent : GameComponent
    {

        [JsonInclude]
        public ScoreType ScoreType { get; set; }

        public event EventHandler ValueModifierChanged;

        [JsonInclude]
        public int ValueModifier { get; set; }

        public ScoreModifierComponent()
        {
        }

        public ScoreModifierComponent(ScoreType scoreType) : base()
        {
            ScoreType = scoreType;
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