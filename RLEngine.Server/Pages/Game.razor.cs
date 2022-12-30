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


        protected InventoryDialog InventoryDialog { get; set; }
        protected InventoryDialogMode Mode { get; set; } = InventoryDialogMode.Player;

        protected System.Threading.Timer timer = null;
        protected long lastTick;
        protected string CommandText { get; set; } = "TEST";
        protected IList<IGameObject> GameBoardObjects = new List<IGameObject>();
        protected ElementReference keyRef;
        protected string pressedKey = "";

        protected bool InventoryDialogOpen = false;
        protected IList<IGameObject> inventoryDialogItems = new List<IGameObject>();

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
            var inventory = player.GetComponent<InventoryComponent>();


            if (InventoryDialogOpen)
            {
                if (pressedKey == "1" || pressedKey == "2")
                {
                    var index = int.Parse(pressedKey) - 1;
                    var item = inventoryDialogItems.ElementAt(index);

                    if (Mode == InventoryDialogMode.GameBoard)
                    {
                        inventory.PickupItem(item);
                        InventoryDialogOpen = false;
                        return true;
                    }
                    if (Mode == InventoryDialogMode.Player)
                    {
                        item.X = player.X;
                        item.Y = player.Y;
                        item.Z = player.Z;

                        inventory.DropItem(item);
                        InventoryDialogOpen = false;
                        return true;
                    }


                }

            }

            switch (command)
            {
                case "ArrowRight":
                case "ArrowLeft":
                case "ArrowDown":
                case "ArrowUp":
                    return GameServer.IssuePlayerMoveCommand(player, command);
                case "i":
                    if (!InventoryDialogOpen)
                    {

                        inventoryDialogItems = inventory.Items;
                        Mode = InventoryDialogMode.Player;
                        InventoryDialogOpen = true;
                        return true;
                    }
                    return true;
                case "p":
                    if (!InventoryDialogOpen)
                    {

                        inventoryDialogItems = GameServer.GetItemsAtPosition(player.X, player.Y, player.Z);
                        if (inventoryDialogItems.Count == 0)
                        {
                            player.AddMessage($"There are no items here.");

                            return false;
                        }
                        Mode = InventoryDialogMode.GameBoard;
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