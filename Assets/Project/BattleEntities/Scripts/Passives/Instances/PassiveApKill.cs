using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassiveApKill : Passive
    {

        public PassiveApKill()
        {
            displayColor = Color.green;
        }

        public override void ExecutedSkill(SkillReport skillreport)
        {
            if (skillreport.TargetAfter.MutableStats[AttributeStats.StatType.Health].Value == 0)
            {
                boardEntity.Stats.AddActionPoints(1);
            }
        }
    }
}
