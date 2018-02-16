using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;
using System;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillAmare2 : Skill
    {
        public SkillAmare2(): base()
        {
            title = "Survivor's Instinct";
            description = "Dodge the next attack against you";
            apCost = 1;
            coolDown = 2;
            range = RANGE_SELF;
        }



        protected override SkillReport ActionHelper(List<Tile> t)
        {
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action<bool> calback = null)
        {
            boardEntity.AddPassive(new BuffDodge(1));
        }
    }
}