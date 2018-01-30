using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffMinusArmour : Buff
    {
        int value;

        public BuffMinusArmour(int stacks, int value): base()
        {
            this.stacks = stacks;
            this.value = value;
            description = "decreases armour by " + value + " for " + stacks + " turns";
        }

        protected override List<StatModifier> GetStatHelperModifiers()
        {
            return new List<StatModifier>() { new StatModifier(StatType.Armour, StatModifierType.Add, -value) };
        }
    }
}
