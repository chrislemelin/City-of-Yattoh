using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Skills
{

    public class SkillJaz1 : Skill
    {

        public SkillJaz1():base()
        {
            title = "True Aim";
            range = RANGE_SELF;
            apCost = 1;
            coolDown = 2;
            description = "Deal increased damage, reduced by movement";
        }


        protected override SkillReport ActionHelper(List<Tile> t)
        {
            boardEntity.AddPassive(new BuffJazAbility1(4));
            return null;
        }
    }
}
