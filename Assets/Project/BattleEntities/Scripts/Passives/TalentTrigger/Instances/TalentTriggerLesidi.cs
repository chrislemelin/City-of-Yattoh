using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentTriggerLesidi : TalentTrigger
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
                Trigger();
            }
            trigger = false;
        }
    }
}
