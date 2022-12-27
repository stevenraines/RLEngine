using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using RLEngine.Core.Enumerations;


namespace RLEngine.Core
{

    public class GameBoard : IGameBoard
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public int Seed { get; set; } = 0;
        public long GameTick { get { return GameLoop?.GameTick ?? 0; } }
        public IGameLoop GameLoop { get; set; }
        public IList<IGameObject> GameObjects { get; set; } = new List<IGameObject>();

        public GameBoard()
        {
        }
        public GameBoard(IGameLoop gameLoop)
        {
            GameLoop = gameLoop;
        }

        public bool AddGameObject(IGameObject gameObject)
        {
            if (gameObject.GameBoard != this)
                throw new System.Exception("Game Object does not belong to this gameboard.");

            if (gameObject.GameBoard.GameObjects.Where(g => g.Id == gameObject.Id).Any())
                throw new System.Exception("Game Object already exists on this gameboard.");

            GameObjects.Add(gameObject);
            return true;
        }

        public IGameObject GetGameObject(Guid Id)
        {
            return GameObjects.Where(x => x.Id == Id).FirstOrDefault();
        }

        public bool AddGameObject(IGameObject gameObject, int x, int y, int z)
        {

            if (!AddGameObject(gameObject))
                return false;

            SetGameObjectPosition(gameObject, x, y, z);
            return true;
        }

        public bool AddGameObject(GameObjectType type, int x, int y, int z)
        {
            return AddGameObject(new GameObject(this, type), x, y, z);
        }

        public bool AddGameObjects(int offsetX, int offsetY, IList<(int x, int y, int z, GameObjectType gameObjectType)> gameObjectPositions)
        {

            bool success = true;

            foreach (var position in gameObjectPositions)
            {
                success = success && AddGameObject(new GameObject(this, position.gameObjectType, offsetX + position.x, offsetY + position.y, position.z, 0));
            }

            return success;
        }

        public IGameBoardPosition GetGameBoardPosition(int x, int y, int z)
        {
            return new GameBoardPosition(x, y, z, GameObjects.Where(g => g.X == x && g.Y == y && g.Z == z).OrderByDescending(g => g.Layer).ThenBy(g => g.Navigable).ToList());
        }

        public bool SetGameObjectPosition(IGameObject gameObject, int x, int y, int z)
        {
            // test if the object can move to the new position
            if (!GetGameBoardPosition(x, y, z).IsNavigable()) return false;

            // if so, move it.
            gameObject.X = x;
            gameObject.Y = y;
            gameObject.Z = z;
            return true;

        }

        public bool MoveGameObject(IGameObject gameObject, int x, int y, int z)
        {

            return SetGameObjectPosition(gameObject, gameObject.X + x, gameObject.Y + y, gameObject.Z + z);

        }

    }


}