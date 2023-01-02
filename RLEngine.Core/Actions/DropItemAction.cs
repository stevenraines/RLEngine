using System;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Extensions;
using RLEngine.Core.Components;
using RLEngine.Core.Attributes;
namespace RLEngine.Core
{

    public class DropItemAction : IAction
    {
        IGameObject Owner { get; }
        IGameObject Item { get; }

        public DropItemAction(IGameObject owner, IGameObject item)
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

            inventory.DropItem(Item, Owner.X, Owner.Y, Owner.Z);
            Owner.Messages.Add(new GameMessage(Owner.GameBoard.GameLoop.GameTick, $"Dropped {Item.Name}"));
            return true;

        }

    }

}