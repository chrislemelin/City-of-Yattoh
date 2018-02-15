using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.AttributeStats;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class TalentTriggerAmare : TalentTrigger
    {

        public TalentTriggerAmare(): base()
        {
            title = "Killing Blow";
            description = "When you kill an enemy, activate talents";

        }

        public override void ExecutedSkill(Skill skill, SkillReport skillreport)
        {
            if(skillreport != null)
            {
                foreach (Tuple<Stats, Stats> statInstance in skillreport.targets)
                {
                    if (statInstance.second.GetMutableStat(StatType.Health).Value == 0)
                    {
                        Trigger();
                    }
                }
            }
 
        }
    }
}
