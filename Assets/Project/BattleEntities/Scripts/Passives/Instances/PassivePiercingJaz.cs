using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;


namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassivePiercingJaz : Passive {

        public PassivePiercingJaz():base()
        {
            title = "Armour Piercing Rounds";
            description = "Pierce enemy armour";
        }

        protected override List<SkillModifier> GetSkillHelperModifiers(Skill skill)
        {
            List<SkillModifier> modifiers = new List<SkillModifier>();
            modifiers.Add(new SkillModifier(SkillModifierType.Piercing, SkillModifierApplication.Add, 
                boardEntity.Stats.GetNonMuttableStat(AttributeStats.StatType.Strength).Value/2 ));
            return modifiers;
        }

    }
}
