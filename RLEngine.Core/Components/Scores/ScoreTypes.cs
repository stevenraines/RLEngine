using RLEngine.Core;
using RLEngine.Core.Events;

namespace RLEngine.Core.Components.Scores
{

    public enum ScoreComposition
    {
        Bonus,
        MaxValueBonus
    }

    public class ScoreType
    {
        public virtual ScoreComposition ScoreComposition { get; set; } = ScoreComposition.Bonus;
    }

    public class HealthScore : ScoreType
    {
        public HealthScore() { ScoreComposition = ScoreComposition.MaxValueBonus; }
    }

    public class DefenseScore : ScoreType { }
    public class AttackScore : ScoreType { }
    public class StrengthScore : ScoreType { }
    public class DexterityScore : ScoreType { }
    public class WisdomScore : ScoreType { }
    public class IntelligenceScore : ScoreType { }
    public class ConstitutionScore : ScoreType { }
    public class CharismaScore : ScoreType { }


}