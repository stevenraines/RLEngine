using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Components;

namespace RLEngine.Core.Factories
{
    public static class ItemFactory
    {

        public static IGameObject CreateItem(IGameBoard gameBoard,
                                                string name,
                                                object properties,
                                                int x,
                                                int y,
                                                int z)
        {
            var item = new GameObject(gameBoard, GameObjectType.Item, name, x, y, z, 1);
            item.AddComponent(new ItemComponent(100));
            return item;
        }

        public static IGameObject CreateWeapon(IGameBoard gameBoard,
                                                string name,
                                                object properties,
                                                int x,
                                                int y,
                                                int z)
        {
            var item = new GameObject(gameBoard, GameObjectType.Item, name, x, y, z, 1);
            item.AddComponent(new ItemComponent(100));
            var validSlots = new HashSet<IEquipmentSlot>();
            validSlots.Add(new EquipmentSlot("Melee Weapon"));
            validSlots.Add(new EquipmentSlot("Alternate Melee Weapon"));
            item.AddComponent(new EquipableComponent(validSlots));
            return item;
        }


    }
}