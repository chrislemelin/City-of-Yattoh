using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Calculator
{

    public class BattleCalculator : MonoBehaviour
    {
        public void DoDamage(CharacterBoardEntity source, CharacterBoardEntity target, DamagePackage damage)
        {
            int newHealth = HealthAfterDamage(source, target, damage);
            SkillReport skillReport = ExecuteSkillHelper(source, target, damage);
            foreach (StatType type in skillReport.SourceAfter.Keys)
            {
                source.Stats.SetMutableStat(type, skillReport.SourceAfter[type].Value);
            }
            foreach (StatType type in skillReport.TargetAfter.Keys)
            {
                target.Stats.SetMutableStat(type, skillReport.TargetAfter[type].Value);
            }           
        }

        public SkillReport ExecuteSkillHelper(CharacterBoardEntity source, CharacterBoardEntity target, DamagePackage damage)
        {
            Dictionary<StatType, Stat> sourceBefore = source.Stats.MutableStats;
            Dictionary<StatType, Stat> sourceAfter = source.Stats.MutableStats;

            Dictionary<StatType, Stat> targetBefore = target.Stats.MutableStats;
            Dictionary<StatType, Stat> targetAfter = target.Stats.MutableStats;

            int newTargetHealth = HealthAfterDamage(source, target, damage);
            
            targetAfter[StatType.Health] = new Stat(targetAfter[StatType.Health], newTargetHealth);

            return new SkillReport(sourceBefore, sourceAfter, targetBefore, targetAfter);

        }

        private int HealthAfterDamage(CharacterBoardEntity source, CharacterBoardEntity target, DamagePackage damage)
        {
            int newHealth = target.Stats.GetMutableStat(Entities.AttributeStats.StatType.Health).Value - damage.Damage;
            if (newHealth < 0)
            {
                // dat bibba dead
                newHealth = 0;
            }
            return newHealth;
        }

    }

   
}
