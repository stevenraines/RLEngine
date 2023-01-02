using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class InventoryDialogBase : ComponentBase
    {

        [Parameter] public EventCallback<IAction> OnActionSelected { get; set; }


        [Parameter]
        public InventoryDialogMode Mode { get; set; } = InventoryDialogMode.Player;

        public InventoryDialogStep Step
        {
            get
            {
                return SelectedItem == null ? InventoryDialogStep.SelectItem : InventoryDialogStep.SelectAction;
            }
        }

        public IGameObject SelectedItem { get; set; } = null;

        public IAction SelectedAction { get; set; } = null;

        public bool Open { get; set; } = false;

        [Parameter]
        public IGameObject Player { get; set; } = null;

        [Parameter]
        public IList<IGameObject> Items { get; set; } = null;

        private InventoryComponent Inventory { get; set; }


        public IList<(char character, IGameObject item)> GetItemSelectionOptions()
        {
            var options = new List<(char character, IGameObject item)>();

            for (var index = 0; index < Items.Count(); index++)
            {
                var indexChar = (char)(index + (int)'a');

                (char character, IGameObject item) option = (indexChar, Items[index]);
                options.Add(option);
            }

            return options;

        }

        public void ToggleDialog()
        {
            Open = !Open;
        }


        public void HandleKeyDown(KeyboardEventArgs e)
        {

            // if escape, close the dialod;
            if (Step == InventoryDialogStep.SelectItem)
            {
                if (e.Key == "Escape") CloseDialog();

                var selection = GetItemSelectionOptions().Where(x => x.character.ToString() == e.Key).FirstOrDefault();

                if (selection != default && selection.item != null)
                {
                    SelectedItem = selection.item;
                    if (SelectedAction is PickUpItemAction)
                    {
                        OnActionSelected.InvokeAsync(new PickUpItemAction(Player, selection.item));
                        CloseDialog();
                        return;
                    }

                    return;
                }

                Player.AddMessage($"'{selection.character.ToString()}' is not a valid selection.");

            }

            if (Step == InventoryDialogStep.SelectAction)
            {
                if (e.Key == "Escape") SelectedItem = null;
                if (e.Key == "d")
                {
                    OnActionSelected.InvokeAsync(new DropItemAction(Player, SelectedItem));
                    CloseDialog();

                }
            }



        }
        public void CloseDialog()
        {

            SelectedItem = null;
            SelectedAction = null;
            Open = false;
        }
    }

    public enum InventoryDialogMode
    {
        Player,
        GameBoard,
        Container

    }

    public enum InventoryDialogStep
    {
        SelectItem,
        SelectAction,
    }

}