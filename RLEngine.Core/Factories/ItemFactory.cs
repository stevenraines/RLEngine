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
            var item = new GameObject(gameBoard, GameObjectType.Item, x, y, z, 1);
            item.Components.Add(new ItemComponent(name, properties));
            return item;
        }

    }
}