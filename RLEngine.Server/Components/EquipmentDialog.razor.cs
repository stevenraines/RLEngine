using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using RLEngine.Core;
using RLEngine.Core.Components;
using RLEngine.Server;
using RLEngine.Server.Components;

namespace RLEngine.Server
{

    public class EquipmentDialogBase : ComponentBase
    {
        [Parameter] public EventCallback<IAction> OnActionSelected { get; set; }
        public EquipmentDialogStep Step
        {
            get
            {
                if (SelectedAction != null) return EquipmentDialogStep.SelectItem;
                if (SelectedSlot != null) return EquipmentDialogStep.SelectAction;

                return EquipmentDialogStep.SelectSlot;
            }
        }
        public IEquipmentSlot SelectedSlot { get; set; } = null;

        public IAction SelectedAction { get; set; } = null;

        public IGameObject SelectedSlotItem { get { return GetSelectedSlotItem(); } }

        public bool Open { get; set; } = false;

        [Parameter]
        public IGameObject Player { get; set; } = null;
        private EquipmentComponent Equipment { get; set; }

        public void ToggleDialog()
        {
            Open = !Open;
        }

        private IGameObject GetSelectedSlotItem()
        {

            return Player.GetComponent<InventoryComponent>().Items.Where(x => x.Id == SelectedSlot.ItemId).FirstOrDefault();

        }
        public void HandleKeyDown(KeyboardEventArgs e)
        {

            // if escape, close the dialod;
            if (Step == EquipmentDialogStep.SelectSlot)
            {
                if (e.Key == "Escape") CloseDialog();

                var selection = GetEquipmentSlotSelectionOptions().Where(x => x.character.ToString() == e.Key).FirstOrDefault();

                if (selection != default && selection.slot != null)
                {
                    SelectedSlot = selection.slot;
                    if (selection.slot.ItemId == null)
                        SelectedAction = new EquipItemAction(Player, null, SelectedSlot);
                    return;
                }

                Player.AddMessage($"'{selection.character.ToString()}' is not a valid selection.");

                return;
            }

            if (Step == EquipmentDialogStep.SelectAction)
            {
                if (e.Key == "Escape")
                {
                    SelectedSlot = null;
                    return;
                }

                if (e.Key == "e")
                {
                    SelectedAction = new EquipItemAction(Player, null, SelectedSlot);
                }

                if (e.Key == "u")
                {
                    SelectedAction = new UnequipItemAction(Player, GetSelectedSlotItem(), SelectedSlot);
                    SelectedAction.Execute();
                    SelectedAction = null;
                    SelectedSlot = null;
                }

                if (e.Key == "d")
                {
                    SelectedAction = new UnequipItemAction(Player, GetSelectedSlotItem(), SelectedSlot);
                    SelectedAction.Execute();
                    SelectedAction = new DropItemAction(Player, GetSelectedSlotItem());
                    SelectedAction.Execute();
                    SelectedAction = null;
                    SelectedSlot = null;

                }
                return;
            }


            if (Step == EquipmentDialogStep.SelectItem)
            {
                if (e.Key == "Escape")
                {
                    SelectedAction = null;
                    var equipCount = Player.GetComponent<InventoryComponent>().GetEquipableItems(SelectedSlot).Count;
                    if (equipCount == 0)
                        SelectedSlot = null;
                    return;
                }


                var selection = GetEquipableItemSelectionOptions().Where(x => x.character.ToString() == e.Key).FirstOrDefault();

                if (selection != default && selection.item != null)
                {
                    ((EquipItemAction)SelectedAction).SetItem(selection.item);
                    SelectedAction.Execute();
                    SelectedAction = null;
                    SelectedSlot = null;
                    return;
                }

                Player.AddMessage($"'{selection.character.ToString()}' is not a valid selection.");

                return;
            }
        }

        public IList<(char character, IGameObject item)> GetEquipableItemSelectionOptions()
        {
            var options = new List<(char character, IGameObject item)>();
            var equipableItems = Player.GetComponent<InventoryComponent>().GetEquipableItems(SelectedSlot);

            if (equipableItems != null)
            {

                for (var index = 0; index < equipableItems.Count(); index++)
                {
                    var indexChar = (char)(index + (int)'a');

                    (char character, IGameObject item) option = (indexChar, equipableItems[index]);
                    options.Add(option);
                }

            }


            return options;

        }


        public IList<(char character, IEquipmentSlot slot, string slotName, string itemName, int applicableItemCount)> GetEquipmentSlotSelectionOptions()
        {
            var options = new List<(char character, IEquipmentSlot slot, string slotName, string itemName, int applicableItemCount)>();
            var equipment = Player.GetComponent<EquipmentComponent>();
            var slots = equipment.Slots;
            var index = 0;

            foreach (var slot in slots)
            {
                var indexChar = (char)(index + (int)'a');

                var itemName = Player.GetComponent<InventoryComponent>().Items.Where(x => x.Id == slot.ItemId).FirstOrDefault()?.Name ?? "None";
                var applicableItemCount = Player.GetComponent<InventoryComponent>().GetEquipableItems(slot).Count;

                (char character, IEquipmentSlot slot, string slotName, string itemName, int applicableItemCount) option = (indexChar, slot, slot.Name, itemName, applicableItemCount);
                options.Add(option);
                index += 1;

            }
            return options;
        }

        public void CloseDialog()
        {
            SelectedSlot = null;
            SelectedAction = null;
            Open = false;
        }
    }

    public enum EquipmentDialogStep
    {
        SelectSlot,
        SelectAction,
        SelectItem
    }

}