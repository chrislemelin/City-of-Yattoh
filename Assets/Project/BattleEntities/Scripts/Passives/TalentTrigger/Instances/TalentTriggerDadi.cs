using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentTriggerDadi : TalentTrigger
    {
        private int lastHealth;
        private float healthThreshold = .25f;

        public TalentTriggerDadi(): base()
        {
            description = "Triggers when you 1/4 health damage since last turn";
        }

        public override void Init(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager)
        {
            base.Init(battleCalculator, boardEntity, tileManager);
            lastHealth = boardEntity.Stats.GetMutableStat(AttributeStats.StatType.Health).Value;
        }

        public override void StartTurn()
        {
            int currentHealthThreshold = Mathf.CeilToInt(boardEntity.Stats.GetNonMuttableStat(AttributeStats.StatType.Health).Value * healthThreshold);
            int lostHealth = lastHealth - boardEntity.Stats.GetMutableStat(AttributeStats.StatType.Health).Value;
            int triggerTimes = lostHealth / currentHealthThreshold;
            if(triggerTimes > 0)
            {
                for(int a = 0; a < triggerTimes;a++)
                {
                    Trigger();
                }
            }
        }

    }
}
