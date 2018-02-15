using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class TalentDadi : Talent
    {

        public TalentDadi()
        {
            title = "Rage";
            description = "Applies rage";
        }
        public override void Activate()
        {
            boardEntity.AddPassive(new BuffDadiRage(2));
        }
    }
}
