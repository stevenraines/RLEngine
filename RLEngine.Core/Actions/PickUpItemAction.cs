using System;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Extensions;
using RLEngine.Core.Components;
using RLEngine.Core.Attributes;
namespace RLEngine.Core
{

    public class PickUpItemAction : IAction
    {
        IGameObject Owner { get; }
        IGameObject Item { get; }

        public PickUpItemAction(IGameObject owner, IGameObject item)
        {
            Owner = owner;
            Item = item;
        }


        public bool Execute()
        {
            var inventory = Owner.GetComponent<InventoryComponent>();

            if (inventory == null)
            {
                Owner.AddMessage($"{Owner.Name} doesn't have an inventory.");
                return false;
            }

            inventory.PickupItem(Item);
            Owner.Messages.Add(new GameMessage(Owner.GameBoard.GameLoop.GameTick, $"Picked up {Item.Name}"));
            return true;

        }

    }

}