using System;
using RLEngine.Core.Enumerations;
using RLEngine.Core.Extensions;
using RLEngine.Core.Attributes;
namespace RLEngine.Core
{

    public class MoveAction : IAction
    {
        IGameObject Owner { get; }
        int X { get; }
        int Y { get; }
        int Z { get; }

        public MoveAction(IGameObject owner, int x, int y, int z)
        {
            Owner = owner;
            X = x;
            Y = y;
            Z = z;
        }

        public MoveAction(IGameObject owner, Direction direction)
        {
            Owner = owner;

            X = direction.GetAttribute<DirectionAttribute>().X;
            Y = direction.GetAttribute<DirectionAttribute>().Y;
            Z = direction.GetAttribute<DirectionAttribute>().Z;
        }

        public bool Execute()
        {
            var success = Owner.Move(X, Y, Z);

            var neighborList = Owner.Neighbors.Where(n =>
                                                        n.gameObject.Id != Owner.Id
                                                        && n.gameObject.Type != GameObjectType.None
                                                        && n.gameObject.Type != GameObjectType.Floor).Select(n => $"{Enum.GetName(n.gameObject.Type)} to the {Enum.GetName(n.direction)}").ToList();

            return success;

        }

    }

}