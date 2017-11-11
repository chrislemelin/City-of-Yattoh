using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        public List<Tile> TileSet()
        {
            return new List<Tile>();
        }
    }
}
