using UnityEngine;
using UnityEditor;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections.Generic;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public abstract class Passive
    {

        protected List<SkillModifier> skillModifiers;

        /// <summary>
        /// for things that cannot be expressed by the skill modifiers, just directly change the damage package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public TakeDamageReturn TakeDamage(Skill skill, DamagePackage package)
        {
            return TakeDamageReturn.Normal;
        }

        /// <summary>
        /// for things that cannot be expressed by the skill modifiers, just directly change the damage package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public DamagePackage DoDamage(DamagePackage package)
        {
            return package;
        }

        public List<SkillModifier> GetSkillModifiers(Skill skill)
        {
            return new List<SkillModifier>();
        }

        public List<SkillModifier> GetSkillModifiers()
        {
            return skillModifiers;
        }

    }

   
}