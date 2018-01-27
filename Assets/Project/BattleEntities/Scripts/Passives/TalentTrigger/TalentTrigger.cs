
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public abstract class TalentTrigger : Passive
    {
        public TalentTrigger() :base()
        {
            type = PassiveType.TalentTrigger;
        }

        public void Trigger()
        {
            boardEntity.TriggerTalents();
        }

    }
}
