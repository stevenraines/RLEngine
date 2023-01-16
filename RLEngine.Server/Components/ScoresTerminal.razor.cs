using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class ScoresTerminalBase : ComponentBase
    {

        [Parameter]
        public IGameObject Player { get; set; } = null;

        public int Health { get { return Player?.GetComponent<HealthScoreComponent>()?.Value ?? 0; } }
        public int MaxHealth { get { return Player?.GetComponent<HealthScoreComponent>()?.GetModifiedMaxValue() ?? 0; } }

    }

}