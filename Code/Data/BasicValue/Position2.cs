using System;
using UnityEngine;

namespace KinglandStudio.NeosKosmos.Data.BasicDataType
{
    [Serializable]
    [SerializeField]
    public class Position2 //二维坐标组
    {
        public float x;
        public float y;
        private static float Abs(float a)
        {
            if(a < 0)
            {
                return -a;
            }
            return a;
        }

        public override bool Equals(object obj)
        {
            return obj is Position2 position &&
                   x == position.x &&
                   y == position.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static Position2 operator +(Position2 a, Position2 b)
        {
            Position2 c = new(a.x + b.x, a.y + b.y);
            return c;
        }
        public static Position2 operator -(Position2 a, Position2 b)
        {
            Position2 c = new(a.x - b.x, a.y - b.y);
            return c;
        }
        public static Position2 operator /(Position2 a, float b)
        {
            Position2 c = new(a.x / b, a.y / b);
            return c;
        }
        public static bool operator ==(Position2 a, Position2 b)
        {
            if(ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Position2 a, Position2 b)
        {
            return !(a == b);
        }
        public Position2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}