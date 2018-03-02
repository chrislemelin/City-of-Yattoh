using System;
using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillTisha2 : Skill
    {
        public SkillTisha2(): base()
        {
            title = "Vengeful Guardian";
            description = "Reflect half of all damage dealt to adjacent allies and self";
            range = RANGE_SELF;
            coolDown = 5;
            apCost = 1;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        {
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action calback = null)
        {
            foreach (BoardEntity boardEntity in TurnManager.Entities)
            {
                if (boardEntity is CharacterBoardEntity && boardEntity.Team == this.boardEntity.Team)
                {
                    boardEntity.AddPassive(new BuffTishaProtect(this.boardEntity, .5f, 2));
                }
            }
            base.ActionHelperNoPreview(tiles, calback);
        }

    }
}
