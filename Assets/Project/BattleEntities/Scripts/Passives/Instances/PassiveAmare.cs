using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassiveAmare : Passive
    {
        public PassiveAmare(): base()
        {
            title = "Quick Thinking";
            description = "Start the battle stealthed";
        }

        public override void StartBattle()
        {
            boardEntity.AddPassive(new BuffStealth(2));
        }

    }
}
