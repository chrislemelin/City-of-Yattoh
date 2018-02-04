using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentBongani : Talent
    {
        public TalentBongani():base()
        {
            description = "applies a buff that will make the next basic attack apply a stun";
        }

        public override void Activate()
        {
            boardEntity.AddPassive(new BuffBonganiTalent());
        }

    }
}