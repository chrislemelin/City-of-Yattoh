using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.AttributeStats;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentTriggerJaz : TalentTrigger
    {

        [SerializeField]
        public float damagePercentage = .5f;

        public TalentTriggerJaz()
        {
            title = "Out For Blood";
            description = "When you damage an enemy for half of their maximum HP, activate talents";
        }

        public override void ExecutedSkill(Skill skill, SkillReport skillReport)
        {
            if (skillReport != null)
            {
                foreach (Tuple<Stats, Stats> report in skillReport.targets)
                {
                    int difference = report.first.GetMutableStat(StatType.Health).Value - report.second.GetMutableStat(StatType.Health).Value;
                    int damageThreshold = Mathf.CeilToInt(report.second.GetNonMuttableStat(StatType.Health).Value * damagePercentage);
                    if (difference >= damageThreshold)
                    {
                        Trigger();
                        //this should only trigger once ?? might change
                        break;
                    }
                }
            }
        }

    }
}
