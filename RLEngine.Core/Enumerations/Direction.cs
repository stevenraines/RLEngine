using RLEngine.Core.Attributes;

namespace RLEngine.Core.Enumerations
{

    public enum Direction
    {
        [Direction(x: 0, y: 0, z: 0)]
        None,

        [Direction(x: -1, y: -1, z: 0)]
        NorthWest,

        [Direction(x: 0, y: -1, z: 0)]
        North,
        [Direction(x: 1, y: -1, z: 0)]
        NorthEast,
        [Direction(x: 1, y: 0, z: 0)]
        East,
        [Direction(x: 1, y: 1, z: 0)]
        SouthEast,
        [Direction(x: 0, y: 1, z: 0)]
        South,
        [Direction(x: -1, y: 0, z: 0)]
        West,
        [Direction(x: -1, y: 1, z: 0)]
        SouthWest,
        [Direction(x: 0, y: 0, z: -1)]
        Down,
        [Direction(x: 0, y: 0, z: 1)]
        Up

    }



}