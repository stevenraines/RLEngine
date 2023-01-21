using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Core.Components.Scores;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class ScoresTerminalBase : ComponentBase
    {

        [Parameter]
        public IGameObject Player { get; set; } = null;

        public ScoreComponent Health { get { return Player?.GetComponent<ScoreComponent>(); } }
        public ScoreComponent Attack { get { return Player?.GetComponent<ScoreComponent<AttackScore>>(); } }
        public ScoreComponent Defense { get { return Player?.GetComponent<ScoreComponent<DefenseScore>>(); } }
        public ScoreComponent Strength { get { return Player?.GetComponent<ScoreComponent<StrengthScore>>(); } }
        public ScoreComponent Dexterity { get { return Player?.GetComponent<ScoreComponent<DexterityScore>>(); } }
        public ScoreComponent Wisdom { get { return Player?.GetComponent<ScoreComponent<WisdomScore>>(); } }
        public ScoreComponent Intelligence { get { return Player?.GetComponent<ScoreComponent<IntelligenceScore>>(); } }
        public ScoreComponent Constitution { get { return Player?.GetComponent<ScoreComponent<ConstitutionScore>>(); } }
        public ScoreComponent Charisma { get { return Player?.GetComponent<ScoreComponent<CharismaScore>>(); } }

    }

}