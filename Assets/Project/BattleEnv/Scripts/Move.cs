using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {

    public int movementCost;
    public Tile destination;
    public List<Tile> path = new List<Tile>();
}
