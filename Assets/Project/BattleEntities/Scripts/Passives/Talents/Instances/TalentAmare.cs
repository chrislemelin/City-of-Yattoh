using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Passives
{ 
    public class TalentAmare : Talent
    {
        public TalentAmare():base()
        {
            title = "Out of sight";
            description = "Activate stealth";
        }

        public override void Activate()
        {
            boardEntity.AddPassive(new BuffStealth(2));
        }

    }
}
