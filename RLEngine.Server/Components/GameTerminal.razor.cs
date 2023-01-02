using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class GameTerminalBase : ComponentBase
    {

        [Parameter]
        public IList<IGameObject> GameBoardObjects { get; set; } = new List<IGameObject>();

        [Parameter]
        public string TerminalWidth { get; set; } = $"{GameServer.DrawDistance * 3 + GameServer.DrawDistance / 2 - 1}em";

        [Parameter]
        public IGameObject Player { get; set; } = null;

        protected string GridColumnString = $"grid-template-columns:{string.Concat(Enumerable.Repeat("auto ", GameServer.DrawDistance * 2 + 1))}";

    }

}