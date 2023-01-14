using System;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Extensions;
using RLEngine.Core.Components;
using RLEngine.Core.Attributes;
namespace RLEngine.Core
{

    public class UnequipItemAction : IAction
    {
        IGameObject Owner { get; }
        IGameObject Item { get; }
        IEquipmentSlot Slot { get; }
        public UnequipItemAction(IGameObject owner, IGameObject item, IEquipmentSlot slot)
        {
            Owner = owner;
            Slot = slot;
            Item = item;
        }


        public bool Execute()
        {
            var inventory = Owner.GetComponent<InventoryComponent>();
            var equipment = Owner.GetComponent<EquipmentComponent>();

            if (inventory == null)
            {
                Owner.AddMessage($"{Owner.Name} doesn't have an inventory.");
                return false;
            }

            if (equipment == null)
            {
                Owner.AddMessage($"{Owner.Name} doesn't have equipment slots.");
                return false;
            }

            equipment.UnequipItem(Slot);
            Owner.Messages.Add(new GameMessage(Owner.GameBoard.GameLoop.GameTick, $"Unequiped {Item.Name} in {Slot.Name}"));
            return true;

        }

    }

}