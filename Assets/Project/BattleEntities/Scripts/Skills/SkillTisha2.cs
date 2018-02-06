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
            title = "Guard Up";
            description = "Mitigate half damage taken by self and allies adjacent and return it back if the target is also adjacent";
            range = RANGE_SELF;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        {
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action<bool> calback = null)
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
