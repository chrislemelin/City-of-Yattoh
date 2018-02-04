using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using System;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillBongani1 : Skill {

        public SkillBongani1():base()
        {
            title = "Build wall";
            description = "makes tile impassible";
            //apCost = 2;
            //coolDown = 4;
            range = RANGE_ADJACENT;
        }

        public override bool TileOptionClickable(Tile t)
        {
            return true;
        }

        public override List<Tile> TileSetHelper(Position p)
        {
            List<Tile> tiles =  base.TileSetHelper(p);
            tiles.RemoveAll((t) => t.canRemove);
            return tiles;
        }

        protected override bool TileHasTarget(Tile t)
        {
            return t.canRemove;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        { 
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action<bool> callback)
        {
            if (tiles.Count > 0)
            {
                tileManager.RemoveTile(tiles[0].Position);
            }
        }

    }
}
