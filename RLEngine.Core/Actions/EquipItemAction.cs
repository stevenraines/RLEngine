using System;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Extensions;
using RLEngine.Core.Components;
using RLEngine.Core.Attributes;
namespace RLEngine.Core
{

    public class EquipItemAction : IAction
    {
        public IGameObject Owner { get; protected set; }
        public IGameObject Item { get; protected set; }
        public IEquipmentSlot Slot { get; protected set; }


        public EquipItemAction(IGameObject owner, IGameObject item, IEquipmentSlot slot)
        {
            Owner = owner;
            Slot = slot;
            Item = item;
        }

        public void SetItem(IGameObject item)
        {
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

            equipment.EquipItem(Item, Slot);

            Owner.Messages.Add(new GameMessage(Owner.GameBoard.GameLoop.GameTick, $"Equiped {Item.Name} in {Slot.Name}"));
            return true;

        }

    }

}