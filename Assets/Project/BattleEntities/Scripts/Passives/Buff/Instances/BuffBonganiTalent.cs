using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffBonganiTalent : Buff
    {

        public BuffBonganiTalent():base()
        {
            description = "Next basic attack will stun";
        }

        public override void ExecutedSkill(Skill skill, SkillReport skillreport)
        {
            if(skill is BasicAttack)
            {
                skillreport.targetAfter.BoardEntity.AddPassive(new BuffStun());
                Remove();
            }
        }
    }
}
