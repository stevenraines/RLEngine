using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class GameBase : ComponentBase
    {

        [Inject] GameServer GameServer { get; set; }
        [Inject] ProtectedLocalStorage protectedLocalStorage { get; set; }

        protected InventoryDialog InventoryDialog { get; set; }
        protected EquipmentDialog EquipmentDialog { get; set; }
        protected HistoryTerminal HistoryTerminal { get; set; }

        protected ScoresTerminal ScoresTerminal { get; set; }

        protected System.Threading.Timer timer = null;
        protected long lastTick;
        protected string CommandText { get; set; } = "";
        protected IList<IGameObject> GameBoardObjects = new List<IGameObject>();
        protected ElementReference keyRef;
        protected string pressedKey = "";
        protected IList<IGameObject> InventoryDialogItems = new List<IGameObject>();

        protected Guid? playerId = null;
        protected IGameObject player;
        private bool terminalFocused = false;

        protected async override void OnInitialized()
        {

            if (GameServer.GameBoard == null) await GameServer.InitializeGameBoard();


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

            }, null, 0, (int)GameServer.GameSpeed / 2);
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

        protected void ScheduleActionHandler<T>(T action) where T : IAction
        {
            player.GameBoard.GameLoop.ScheduleAction(new ScheduledAction(player.Id, action));
        }

        protected void CommandChangeHandler(string command)
        {
            if (!terminalFocused) return;
            CommandText = command;
            ParseTextCommand(command);
            CommandText = "";
            terminalFocused = false;
            keyRef.FocusAsync();

        }

        protected void HistoryTerminalClicked()
        {
            HistoryTerminal.Focus();
            terminalFocused = true;

        }

        protected void ParseTextCommand(string command)
        {

            if (player != null)
                player.Messages.Add(new GameMessage(GameServer.GameTick, $"Issued command ({command})"));

        }

        protected void HandleKeyDown(KeyboardEventArgs e)
        {


            if (player == null) return;


            if (IssuePlayerCommand(player, e))
            {
                CommandText = "";
            }

        }


        protected bool IssuePlayerCommand(IGameObject player, KeyboardEventArgs e)
        {
            var command = e.Key;

            if (terminalFocused)
            {
                CommandText = "";
                if (command == "Escape")
                {
                    terminalFocused = false;
                    keyRef.FocusAsync();
                }


                return false;
            }

            keyRef.FocusAsync();

            if (e.Key == "`")
            {
                HistoryTerminalClicked();
            }

            // if the equipment dialog is open, route command there.
            if (EquipmentDialog.Open)
                return HandleEquipmentDialogAction(e);

            // if the inventory dialog is open, route command there.
            if (InventoryDialog.Open)
                return HandleInventoryDialogAction(e);

            // if the CTRL key is pressed, do alternate actions
            if (e.CtrlKey)
                return HandlePlayerCtrlAction(player, e);

            // handle unmodified actions.
            return HandlePlayerStandardAction(player, e);

        }

        public bool HandleEquipmentDialogAction(KeyboardEventArgs e)
        {
            // allow the dialog to handle keypresses until closed;
            EquipmentDialog.HandleKeyDown(e);
            return true;
        }

        public bool HandleInventoryDialogAction(KeyboardEventArgs e)
        {
            // allow the dialog to handle keypresses until closed;
            InventoryDialog.HandleKeyDown(e);
            return true;
        }

        public bool HandlePlayerCtrlAction(IGameObject player, KeyboardEventArgs e)
        {
            var command = e.Key;
            return false;
        }

        public bool HandlePlayerStandardAction(IGameObject player, KeyboardEventArgs e)
        {
            var command = e.Key;

            switch (command)
            {
                case "ArrowRight":
                case "ArrowLeft":
                case "ArrowDown":
                case "ArrowUp":
                    return GameServer.IssuePlayerMoveCommand(player, command);
                case "e":
                    if (!EquipmentDialog.Open)
                    {
                        EquipmentDialog.ToggleDialog();
                    }
                    return true;
                case "i":
                    if (!InventoryDialog.Open)
                    {
                        InventoryDialogItems = player.GetComponent<InventoryComponent>().Items;
                        InventoryDialog.Mode = InventoryDialogMode.Player;
                        InventoryDialog.ToggleDialog();
                        return true;
                    }
                    return true;
                case "p":
                    if (!InventoryDialog.Open)
                    {

                        InventoryDialogItems = GameServer.GetItemsAtPosition(player.X, player.Y, player.Z);
                        if (InventoryDialogItems.Count == 0)
                        {
                            player.AddMessage($"There are no items here.");

                            return false;
                        }

                        InventoryDialog.SelectedAction = new PickUpItemAction(player, null);
                        InventoryDialog.Mode = InventoryDialogMode.GameBoard;
                        InventoryDialog.ToggleDialog();
                        return true;
                    }
                    return true;

                case "Escape":
                    return true;
            }

            return false;

        }
    }
}