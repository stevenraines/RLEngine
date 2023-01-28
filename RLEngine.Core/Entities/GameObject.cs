using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Attributes;
using RLEngine.Core.Extensions;
using RLEngine.Core.Components;

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

        [NotMapped]
        public IDictionary<string, dynamic> Components { get; set; } = new Dictionary<string, dynamic>();

        public IList<(Direction direction, IGameObject gameObject)> Neighbors { get { return GetNeighbors(); } }

        public Guid ContainerGameObjectId { get; set; } = Guid.Empty;

        public bool Navigable
        {
            get
            {
                return Type.GetAttribute<NavigableAttribute>().Navigable;
            }
        }


        public GameObject() { }

        public GameObject(IGameBoard gameBoard, GameObjectType type)
        {
            GameBoard = gameBoard;
            GameBoardId = gameBoard.Id;
            Type = type;

        }

        public GameObject(IGameBoard gameBoard, GameObjectType type, string name, int x, int y, int z, int layer) : this(gameBoard, type, x, y, z, layer)
        {
            Name = String.IsNullOrEmpty(name) ? $"GameObject {Enum.GetName(type)} {Id.ToString()}" : name;
        }
        public GameObject(IGameBoard gameBoard, GameObjectType type, int x, int y, int z, int layer) : this(gameBoard, type)
        {
            GameBoard.SetGameObjectPosition(this, x, y, z);
            Layer = layer;

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

        public void AddMessage(string message)
        {
            Messages.Add(new GameMessage(GameBoard.GameLoop.GameTick, message));
        }

        public T AddComponent<T>(T component)
        {

            if (HasComponent<T>()) throw new Exception("Component already exists");

            if ((component) is GameComponent)
            {
                ((IGameComponent)component).GameObject = this;
            }

            Components.Add(typeof(T).AssemblyQualifiedName, component);
            return component;

        }
        public T GetComponent<T>()
        {

            var trueKeyName = Components.Keys.Where(x => x == typeof(T).AssemblyQualifiedName).FirstOrDefault();
            if (trueKeyName == null) return default(T);
            return Components.Where(x => x.Key == trueKeyName).Select(x => x.Value).FirstOrDefault();
        }


        public bool HasComponent<T>()
        {

            return Components.Keys.Any(x => x == typeof(T).AssemblyQualifiedName);

        }


        public string SerializedComponents
        {
            get
            {
                var components = JsonSerializer.Serialize(Components);
                return components;
            }
            set
            {

                var componentList = new Dictionary<string, dynamic>();

                if (string.IsNullOrEmpty(value)) return;
                var components = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(value);

                if (components == null || components.Count == 0) return;

                foreach (var component in components)
                {

                    try
                    {

                        var componentTypeName = component.Key.ToString();
                        Type T = System.Type.GetType(componentTypeName);
                        var obj = (JsonElement)component.Value;
                        var options = new JsonSerializerOptions();
                        options.IncludeFields = true;
                        options.IgnoreReadOnlyFields = true;

                        var cmp = JsonSerializer.Deserialize(obj.GetRawText(), T, options);
                        if (cmp is IGameComponent)
                            ((IGameComponent)cmp).GameObject = this;
                        componentList.Add(componentTypeName, cmp);


                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                    }

                }

                Components = componentList;
            }
        }

    }

}