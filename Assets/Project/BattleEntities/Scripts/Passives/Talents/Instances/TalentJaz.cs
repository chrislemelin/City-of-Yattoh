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
            title = "Eagle Eye";
            type = PassiveType.Talent;
            description = "Next attack will have double range";

        }

        public override void Activate()
        {
            //Passive
            boardEntity.AddPassive(new BuffTalentJaz());
        }

    }
}
