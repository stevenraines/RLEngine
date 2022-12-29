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

        public string Name { get; set; } = "";
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Layer { get; set; }
        public GameObjectType Type { get; set; }
        public Guid GameBoardId { get; set; }
        public IGameBoard GameBoard { get; set; }
        public IList<IGameMessage> Messages { get; set; } = new List<IGameMessage>();
        public IList<IGameComponent> Components { get; set; } = new List<IGameComponent>();

        public IList<(Direction direction, IGameObject gameObject)> Neighbors { get { return GetNeighbors(); } }


        public bool Navigable
        {
            get
            {
                return Type.GetAttribute<NavigableAttribute>().Navigable;
            }
        }


        public GameObject() { }

        public GameObject(IGameBoard gameBoard, GameObjectType type, string name = null)
        {
            GameBoard = gameBoard;
            Type = type;
            Name = name ?? $"GameObject {Enum.GetName(type)} {Id.ToString()}";
        }
        public GameObject(IGameBoard gameBoard, GameObjectType type, int x, int y, int z, int layer) : this(gameBoard, type)
        {
            GameBoard.SetGameObjectPosition(this, x, y, z);

        }

        public bool Move(int x, int y, int z)
        {

            return GameBoard.MoveGameObject(this, x, y, z);
        }

        private IList<(Direction direction, IGameObject gameObject)> GetNeighbors()
        {

            // return the game objects where the x is within one of the
            var neighbors = GameBoard.GameObjects.Where(g => g.Id != Id
                                                                && g.X <= X + 1
                                                                && g.X >= X - 1
                                                                && g.Y <= Y + 1
                                                                && g.Y >= Y - 1
                                                                && g.Z == Z
                                                                && g.Type != GameObjectType.Floor).ToList();

            var neighborTypes = new List<(Direction direction, IGameObject gameObject)>();

            var directions = Direction.GetValues<Direction>();

            foreach (var neighbor in neighbors)
            {
                var direction = directions.Where(d => d.GetAttribute<DirectionAttribute>().X == neighbor.X - X
                                    && d.GetAttribute<DirectionAttribute>().Y == neighbor.Y - Y
                                    && d.GetAttribute<DirectionAttribute>().Z == neighbor.Z
                                    ).FirstOrDefault();

                neighborTypes.Add((direction, neighbor));
            }

            return neighborTypes;
        }

    }

}