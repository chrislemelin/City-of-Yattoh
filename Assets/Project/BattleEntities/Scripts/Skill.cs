using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour {

    public List<Tile> TileSet()
    {
        return new List<Tile>();
    }
}
