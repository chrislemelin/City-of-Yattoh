using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle
{
    public class Move {

        public int movementCost;
        public Tile destination;
        public List<Tile> path = new List<Tile>();
    }
}
