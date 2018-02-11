using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassiveAmare : Passive
    {
        public PassiveAmare(): base()
        {
            title = "Battle Ready";
            description = "start the battle stealthed";
        }

        public override void StartBattle()
        {
            boardEntity.AddPassive(new BuffStealth(2));
        }

    }
}
