using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Core.Enumerations;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class GameTerminalBase : ComponentBase
    {

        [Parameter]
        public IList<IGameObject> GameBoardObjects { get; set; } = new List<IGameObject>();

        [Parameter]
        public int TerminalWidth { get; set; } =  GameServer.DrawDistance * 3 / 2;

        [Parameter]
        public int TerminalHeight { get; set; } = GameServer.DrawDistance;

        [Parameter]
        public IGameObject Player { get; set; } = null;

        protected string GridColumnString = $"grid-template-columns:{string.Concat(Enumerable.Repeat("auto ", GameServer.DrawDistance * 2 + 1))}";

        public MarkupString RenderView()
        {

            if (Player == null) return (MarkupString)"";
            string render = "";
            var startX = Player.X - TerminalWidth;
            var startY = Player.Y - TerminalHeight;

            for (var y = startY; y < TerminalHeight - 1; y++)
            {

                for (var x = startX; x < TerminalWidth - 1; x++)
                {
                    var gameBoardObject = GameBoardObjects.Where(g => g.Y == y && g.X == x).FirstOrDefault();
                    var positionType = gameBoardObject?.Type ?? GameObjectType.None;
                    string str = ((char)(int)positionType).ToString();
                    render += $"{str}";
                  
                }


                render += "<br>";
            }
            /*
            foreach (var gameBoardObject in GameBoardObjects.OrderBy(g => g.Y).ThenBy(g => g.X).ToList())
            {

               
            }
            */
            return (MarkupString) render; ;


        }
    }
}