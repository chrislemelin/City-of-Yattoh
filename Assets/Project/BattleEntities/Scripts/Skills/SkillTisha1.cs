using System;
using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillTisha1 : Skill
    {

        public SkillTisha1():base()
        {
            title = "Ironclad Paragon";
            description = "All allies gain armour for each adjacent enemy";
            range = RANGE_SELF;
            coolDown = 3;
            apCost = 2;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        {
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action callback = null)
        {
            List<CharacterBoardEntity> enemies = tileManager.TilesToCharacterBoardEntities(tileManager.GetTilesDiag(boardEntity.Position, 1),boardEntity.Team);
            int moreArmour = enemies.Count;
            foreach(BoardEntity boardEntity in TurnManager.Entities)
            {
                if(boardEntity is CharacterBoardEntity && boardEntity.Team == this.boardEntity.Team)
                {
                    boardEntity.AddPassive(new BuffArmour(moreArmour, 2));
                }
            }
            base.ActionHelperNoPreview(tiles, callback);
        }

    }
}
