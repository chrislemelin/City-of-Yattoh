using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Calculator
{

    public class BattleCalculator : MonoBehaviour
    {
        public void DoDamage(CharacterBoardEntity source, Skill skill, CharacterBoardEntity target, DamagePackage damage)
        {
            SkillReport skillReport = ExecuteSkillHelper(source, skill, target, damage);
            //source.Stats = skillReport.SourceAfter;

            if (skillReport != null)
            {
                foreach (StatType type in skillReport.SourceAfter.MutableStats.Keys)
                {
                    source.Stats.SetMutableStat(type, skillReport.SourceAfter.MutableStats[type].Value);
                }
                foreach (StatType type in skillReport.TargetAfter.MutableStats.Keys)
                {
                    target.Stats.SetMutableStat(type, skillReport.TargetAfter.MutableStats[type].Value);
                }
            }
        }

        public SkillReport ExecuteSkillHelper(CharacterBoardEntity source, Skill skill, CharacterBoardEntity target, DamagePackage damage)
        {
            Stats sourceBefore = source.Stats.GetCopy();
            Stats sourceAfter = source.Stats.GetCopy();

            Stats targetBefore = target.Stats.GetCopy();
            Stats targetAfter = target.Stats.GetCopy();

            TakeDamageReturn usingTakeDamageReturn = TakeDamageReturn.Normal;
            List<Passive> passives = target.Passives;
            foreach(Passive passive in passives)
            {
                TakeDamageReturn currentReturn = passive.TakeDamage(skill, damage);
                usingTakeDamageReturn = (TakeDamageReturn)Mathf.Max((int)currentReturn, (int)usingTakeDamageReturn);
            }

            switch(usingTakeDamageReturn)
            {
                case TakeDamageReturn.Normal:
                    int newTargetHealth = HealthAfterDamage(source, target, damage);
                    targetAfter.SetMutableStat(StatType.Health, newTargetHealth);
                    return new SkillReport(sourceBefore, sourceAfter, targetBefore, targetAfter);

                case TakeDamageReturn.NoDamage:
                    return null;                    

                case TakeDamageReturn.Reflect:
                    return null;
            }

            return null;

        }

        private int HealthAfterDamage(CharacterBoardEntity source, CharacterBoardEntity target, DamagePackage damage)
        {
            int newHealth = target.Stats.GetMutableStat(StatType.Health).Value - damage.Damage;
            if (newHealth < 0)
            {
                // dat bibba dead
                newHealth = 0;
            }
            return newHealth;
        }

    }

    public enum TakeDamageReturn
    {
        Normal, NoDamage, Reflect
    }
}
