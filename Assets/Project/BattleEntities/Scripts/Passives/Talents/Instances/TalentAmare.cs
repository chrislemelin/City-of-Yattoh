using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Passives
{ 
    public class TalentAmare : Talent
    {
        public TalentAmare():base()
        {
            title = "stealth";
            description = "TALENT make yourself stealthed";
        }

        public override void Activate()
        {
            boardEntity.AddPassive(new BuffStealth(2));
        }

    }
}
