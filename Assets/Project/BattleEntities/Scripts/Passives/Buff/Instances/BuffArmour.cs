using Placeholdernamespace.Battle.Entities.AttributeStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffArmour : Buff
    {
        private int value;

        public BuffArmour(int value, int stacks): base(stacks)
        {
            description = "add " + value + " to armour";
            statModifiers = new List<StatModifier>() { new StatModifier(StatType.Armour, StatModifierType.Add, value)};
        }



    }
}