using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;
using System;

namespace Placeholdernamespace.Battle.Entities.Skills
{

    public class SkillBongani3 : Skill
    {

        public SkillBongani3(): base()
        {
            title = "Ocean's Embrace";
            description = "stun an enemy and all adjacent enemies";
            range = RANGE_ADJACENT;
            coolDown = 3;
            apCost = 1;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        {
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action callback)
        {
            List<Tile> targetTiles = tileManager.GetTilesDiag(tiles[0].BoardEntity.Position);
            List<CharacterBoardEntity> targets = tileManager.TilesToCharacterBoardEntities(targetTiles);
            targets.Add((CharacterBoardEntity)tiles[0].BoardEntity);
            foreach (CharacterBoardEntity character in targets)
            {
                character.AddPassive(new BuffStun());
            }
            base.ActionHelperNoPreview(tiles, callback);
        }

        public override List<Tile> TileReturnHelper(Tile t)
        {
            List<Tile> returnList = new List<Tile>();
            returnList.Add(t);
            returnList.AddRange(tileManager.GetTilesDiag(t.Position));
            
            return returnList;
        }


    }
}
