using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class GameBase : ComponentBase
    {

        [Inject] GameServer GameServer { get; set; }
        [Inject] ProtectedLocalStorage protectedLocalStorage { get; set; }

        protected System.Threading.Timer timer = null;
        protected long lastTick;
        protected string CommandText { get; set; } = "TEST";
        protected IList<IGameObject> GameBoardObjects = new List<IGameObject>();
        protected ElementReference keyRef;
        protected string pressedKey = "";

        protected bool InventoryDialogOpen = false;
        protected IList<IGameObject> pickupItems = new List<IGameObject>();

        protected Guid? playerId = null;
        protected IGameObject player;

        protected override void OnInitialized()
        {

            timer = new System.Threading.Timer(async _ =>  // async void
            {

                if (GameServer.GameTick <= lastTick) return;

                if (player != null)
                {

                    GameBoardObjects = GameServer.GetGameBoardObjectsToRender(player.X, player.Y, player.Z);

                    // we need StateHasChanged() because this is an async void handler
                    // we need to Invoke it because we could be on the wrong Thread
                    await InvokeAsync(StateHasChanged);
                }

            }, null, 0, (int)GameServer.GameTimer.Interval / 2);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                await SetPlayer();
            }
            // await keyRef.FocusAsync();
            lastTick = GameServer.GameTick;

        }

        protected async Task<Guid> SetPlayer()
        {

            // see if a player is already registered.
            if (playerId != null && playerId != Guid.Empty)
            {
                return (Guid)playerId;
            }

            playerId = (await protectedLocalStorage.GetAsync<Guid>("PlayerId")).Value;


            // see if a player is already registered.
            player = await GameServer.RegisterPlayer((Guid)playerId);
            await protectedLocalStorage.SetAsync("PlayerId", player.Id);
            playerId = player.Id;

            return (Guid)player.Id;

        }

        protected void CommandChangeHandler(string command)
        {
            CommandText = command;
            ParseTextCommand(command);
            CommandText = "";
        }

        protected void ParseTextCommand(string command)
        {

            if (player != null)
                player.Messages.Add(new GameMessage(GameServer.GameTick, $"Issued command ({command})"));

        }


        protected void HandleKeyDown(KeyboardEventArgs e)
        {
            pressedKey = e.Key;
            if (player != null)
            {
                if (IssuePlayerCommand(player, pressedKey))
                {
                    CommandText = "";
                }

            }

        }


        protected bool IssuePlayerCommand(IGameObject player, string command)
        {

            switch (command)
            {
                case "ArrowRight":
                case "ArrowLeft":
                case "ArrowDown":
                case "ArrowUp":
                    return GameServer.IssuePlayerMoveCommand(player, command);
                case "p":
                    if (!InventoryDialogOpen)
                    {

                        pickupItems = GameServer.GetItemsAtPosition(player.X, player.Y, player.Z);
                        if (pickupItems.Count == 0)
                        {
                            player.AddMessage($"There are no items here.");

                            return false;
                        }

                        InventoryDialogOpen = true;
                        return true;
                    }
                    return true;
                case "Escape":
                    InventoryDialogOpen = false;
                    return true;
            }
            return false;
        }

    }
}