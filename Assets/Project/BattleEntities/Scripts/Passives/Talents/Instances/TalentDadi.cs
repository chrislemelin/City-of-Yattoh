using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class TalentDadi : Talent
    {

        public TalentDadi()
        {
            title = "Savage Spirit";
            description = "Rage for two turns, letting you attack without cooldown before becoming exhausted";
        }
        public override void Activate()
        {
            boardEntity.AddPassive(new BuffDadiRage(2));
        }
    }
}
