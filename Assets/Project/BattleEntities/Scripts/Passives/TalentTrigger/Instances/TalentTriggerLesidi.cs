using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentTriggerLesidi : Passive
    {

        bool trigger = false;

        public override void ExecutedMove(Move move)
        {
            trigger = true;
        }

        public override void EndTurn()
        {
            if(trigger)
            {
                boardEntity.TriggerTalents();
            }
            trigger = false;
        }
    }
}
