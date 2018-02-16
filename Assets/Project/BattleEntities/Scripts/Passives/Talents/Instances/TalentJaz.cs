using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives 
{
    public class TalentJaz : Talent {

        public TalentJaz()
        {
            title = "Long Arm of the Law";
            type = PassiveType.Talent;
            description = "Next attack has double range";

        }

        public override void Activate()
        {
            //Passive
            boardEntity.AddPassive(new BuffTalentJaz());
        }

    }
}
