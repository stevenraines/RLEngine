using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RLEngine.Core.Enumerations;


namespace RLEngine.Core
{

    public interface IGameObject
    {
        Guid Id { get; }
        string Name { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Z { get; set; }
        int Layer { get; set; }
        GameObjectType Type { get; }
        Guid GameBoardId { get; set; }
        IGameBoard GameBoard { get; }

        Guid ContainerGameObjectId { get; set; }

        [NotMapped]
        IDictionary<string, dynamic> Components { get; set; }

        //  IList<IGameComponent> Components { get; set; }
        IList<IGameMessage> Messages { get; set; }
        bool Navigable { get; }
        IList<(Direction direction, IGameObject gameObject)> Neighbors { get; }
        bool Move(int x, int y, int z);
        void AddMessage(string message);

        T AddComponent<T>(T component);
        T GetComponent<T>();
        public bool HasComponent<T>();
        public string SerializedComponents { get; set; }
    }

}