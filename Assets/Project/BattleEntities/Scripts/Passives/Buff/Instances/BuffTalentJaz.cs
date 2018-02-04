using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class BuffTalentJaz : Buff
    {

        public BuffTalentJaz(): base()
        {
            skillModifiers.Add(new SkillModifier(SkillModifierType.Range, SkillModifierApplication.Mult, 2));
            description = "Next attack has double range";
        }

        public override void ExecutedSkill(Skill skill, SkillReport skillreport)
        {
            base.ExecutedSkill(skill, skillreport);
            Remove();

        }

    }
}
