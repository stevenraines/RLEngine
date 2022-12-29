using System.Collections.Generic;
using RLEngine.Core.Enumerations;

namespace RLEngine.Core
{

    public interface IGameBoard
    {

        Guid Id { get; set; }
        int Seed { get; set; }

        IGameLoop GameLoop { get; set; }
        IList<IGameObject> GameObjects { get; set; }

        IGameObject GetGameObject(Guid Id);
        IGameObject AddGameObject(IGameObject gameObject);
        IGameObject AddGameObject(IGameObject gameObject, int x, int y, int z);
        IGameObject AddGameObject(GameObjectType type, int x, int y, int z);
        IList<IGameObject> AddGameObjects(int x, int y, IList<(int x, int y, int z, GameObjectType gameObjectType)> gameObjectPositions);


        bool SetGameObjectPosition(IGameObject gameObject, int x, int y, int z);
        IGameBoardPosition GetGameBoardPosition(int x, int y, int z);
        bool MoveGameObject(IGameObject gameObject, int x, int y, int z);
    }

}