using System;
using System.Collections.Generic;
using RLEngine.Core.Enumerations;

using RLEngine.Core.Attributes;
using RLEngine.Core.Extensions;


namespace RLEngine.Core
{

    public class GameObject : IGameObject
    {
        // the unique id of the entity
        public Guid Id { get; set; } = Guid.NewGuid();
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Layer { get; set; }
        public GameObjectType Type { get; set; }
        public IGameBoard GameBoard { get; }
        public IList<IGameComponent> Components { get; set; }

        public bool Navigable
        {
            get
            {
                return Type.GetAttribute<NavigableAttribute>().Navigable;
            }
        }

        public GameObject(IGameBoard gameBoard, GameObjectType type)
        {
            GameBoard = gameBoard;
            Type = type;
        }
        public GameObject(IGameBoard gameBoard, GameObjectType type, int x, int y, int z, int layer) : this(gameBoard, type)
        {
            GameBoard.SetGameObjectPosition(this, x, y, z);

        }

        public bool Move(int x, int y, int z)
        {

            return GameBoard.MoveGameObject(this, x, y, z);
        }

    }

}