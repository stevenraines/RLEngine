using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Components;

namespace RLEngine.Core.Factories
{
    public static class ItemFactory
    {

        public static IGameObject CreateItem(IGameBoard gameBoard)
        {
            var item = new GameObject(gameBoard, GameObjectType.Item);
            item.Components.Add(new ItemComponent());
            return item;
        }
        public static IGameObject CreateItem(IGameBoard gameBoard, int x, int y, int z)
        {
            return gameBoard.AddGameObject(CreateItem(gameBoard, x, y, z));


        }
    }
}