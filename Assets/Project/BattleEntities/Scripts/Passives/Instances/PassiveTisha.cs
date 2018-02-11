using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives {

    public class PassiveTisha : Passive {

        public PassiveTisha(): base()
        {
            title = "Unkillable";
            description = "Revive self once a game";
           
        }

        public override void Die()
        {
            boardEntity.Stats.SetMutableStat(AttributeStats.StatType.Health,
                boardEntity.Stats.GetNonMuttableStat(AttributeStats.StatType.Health).Value);
            
            boardEntity.FloatingTextGenerator.AddTextDisplay(new Common.UI.TextDisplay() { text = "Revived" });
            boardEntity.RemovePassive(this);
        }

    }
}
