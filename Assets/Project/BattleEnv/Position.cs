using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Env
{
    public class Position
    {

        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position(int length)
        {
            this.x = length;
            this.y = length;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() == this.GetType())
            {
                Position posCompare = (Position)obj;
                return posCompare.x == this.x && posCompare.y == this.y;
            }
            return false;
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.x + p2.x, p1.y + p2.y);
        }

        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.x - p2.x, p1.y - p2.y);
        }

        public float GetDistance(Position position)
        {
            return Mathf.Sqrt(Mathf.Pow(x - position.x, 2) + Mathf.Pow(y - position.y, 2));
        }

    }
}