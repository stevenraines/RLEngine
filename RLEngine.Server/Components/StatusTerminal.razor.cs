using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class StatusTerminalBase : ComponentBase
    {

        [Parameter]
        public IGameObject Player { get; set; } = null;

        [Parameter]
        public string LastAction { get; set; } = "";

        [Parameter]
        public string TerminalWidth { get; set; } = $"{30 - (GameServer.DrawDistance * 2 + 4)}em";



    }

}