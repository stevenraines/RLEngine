using System;
using System.Reflection;

namespace RLEngine.Core.Attributes
{

    public class DirectionAttribute : Attribute
    {


        protected (int x, int y, int z) _vector = (0, 0, 0);


        public DirectionAttribute(int x, int y, int z)
        {
            _vector = (x, y, z);
        }

        public (int x, int y, int z) Vector
        {
            get { return _vector; }
        }

        public int X { get { return _vector.x; } }
        public int Y { get { return _vector.y; } }
        public int Z { get { return _vector.z; } }


    }

}