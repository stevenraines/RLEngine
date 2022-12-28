using System.Text;
using RLEngine.Core.Attributes;

namespace RLEngine.Core.Enumerations
{
    public enum GameObjectType

    {
        [Navigable(true)]
        None = (int)' ',

        [Navigable(true)]
        Item = (int)'/',

        [Navigable(true)]
        Floor = (int)'·', //225,

        [Navigable(false)]
        Player = (int)'@', //64

        [Navigable(false)]
        Monster = (int)'M', //??

        [Navigable(false)]
        Wall = (int)'#',

        [Navigable(true)]
        OpenDoor = 47,

        [Navigable(false)]
        ClosedDoor = 43,


    }
}