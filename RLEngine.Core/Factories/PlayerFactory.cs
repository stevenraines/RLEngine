using RLEngine.Core;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Components;

namespace RLEngine.Core.Factories
{
    public static class PlayerFactory
    {

        public static IGameObject CreatePlayer(IGameBoard gameBoard)
        {
            var player = new GameObject(gameBoard, GameObjectType.Player, "Player1", 0, 0, 0, 10);
            player.Components.Add(typeof(InventoryComponent).AssemblyQualifiedName, new InventoryComponent());

            return player;
        }

        public static IGameObject CreatePlayer(IGameBoard gameBoard, int x, int y, int z)
        {
            var player = CreatePlayer(gameBoard);
            gameBoard.AddGameObject(player, x, y, z);

            return player;
        }

    }
}