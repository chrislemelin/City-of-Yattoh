using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle
{
    public class Move {

        public int movementCost;
        public int apCost;
        public int movementPointsAfterMove;
        public Tile destination;
        public List<Tile> path = new List<Tile>();

        public override bool Equals(object obj)
        {
            if(obj is Move)
            {
                Move move = (Move)obj;
                return (move.movementCost.Equals(movementCost) && move.destination.Equals(destination));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return destination.GetHashCode();
        }
    }
}
