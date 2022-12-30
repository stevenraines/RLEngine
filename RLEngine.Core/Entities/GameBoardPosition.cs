using System;
using System.Linq;
using System.Collections.Generic;
using RLEngine.Core.Enumerations;


namespace RLEngine.Core
{

    public class GameBoardPosition : IGameBoardPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public IList<IGameObject> GameObjects { get; set; } = new List<IGameObject>();

        public IList<(Direction direction, IGameObject gameObject)> Neightbors { get; set; } = new List<(Direction direction, IGameObject gameObject)>();


        public GameBoardPosition(int x, int y, int z, IList<IGameObject> gameObjects)
        {
            X = x;
            Y = y;
            Z = z;
            GameObjects = gameObjects;

        }

        public IList<IGameObject> GetGameObjectsWithComponent<T>()
        {
            var objectsOfType = GameObjects.Where(x => x.GetComponent<T>() != null).ToList();
            return objectsOfType;
        }

        public bool IsNavigable()
        {
            return GameObjects.All(g => g.Navigable);
        }

    }

}