using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffDadiRage : Buff
    {

        public BuffDadiRage(int stacks): base(stacks)
        {
            addBuffHandle = AddBuff.refresh;
            description = "gets rid of cool down on basic attack";
        }

        protected override List<SkillModifier> GetSkillHelperModifiers(Skill skill)
        {
            if(skill is BasicAttack)
            {
                return new List<SkillModifier>() { new SkillModifier(SkillModifierType.CoolDown, SkillModifierApplication.Add, -1) };
            }
            return new List<SkillModifier>() { };
        }

        protected override void RemoveHelper()
        {
            //boardEntity.AddPassive();
        }

    }
}
