using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class BasicAttack : Skill
    {
        public BasicAttack()
        {
            title = "Basic Attack";
        }

        public override List<Tile> TileSet()
        {
            return tileManager.GetAllTilesNear(boardEntity.GetTile().Position);
        }

        public override void Action(Tile t)
        {
            MonoBehaviour.print("woop an attack at "+t.Position);
        }

    }
}
