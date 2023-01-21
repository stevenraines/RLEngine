using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Components.Scores;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class ScoresTerminalBase : ComponentBase
    {

        [Parameter]
        public IGameObject Player { get; set; } = null;

        public int Health { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.HealthScore) ?? 0; } }
        public int MaxHealth { get { return Player?.GetComponent<ScoreComponent>().GetCurrentMaxValue(ScoreType.HealthScore) ?? 0; } }
        public int Attack { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.AttackScore) ?? 0; } }
        public int Defense { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.DefenseScore) ?? 0; } }
        public int Strength { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.StrengthScore) ?? 0; } }
        public int Dexterity { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.DefenseScore) ?? 0; } }
        public int Wisdom { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.WisdomScore) ?? 0; } }
        public int Intelligence { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.IntelligenceScore) ?? 0; } }
        public int Constitution { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.ConstitutionScore) ?? 0; } }
        public int Charisma { get { return Player?.GetComponent<ScoreComponent>().GetCurrentValue(ScoreType.CharismaScore) ?? 0; } }

    }

}