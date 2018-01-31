using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class BuffLesidiAbility2 : Buff
    {
        public BuffLesidiAbility2():base(2)
        {
            addBuffHandle = AddBuff.refresh;
            description = "+1 to all stats for 2 turns";
            statModifiers = new List<AttributeStats.StatModifier>()
            {
                new AttributeStats.StatModifier(AttributeStats.StatType.Armour,AttributeStats.StatModifierType.Add,1),
                new AttributeStats.StatModifier(AttributeStats.StatType.Movement,AttributeStats.StatModifierType.Add,1),
                new AttributeStats.StatModifier(AttributeStats.StatType.Speed,AttributeStats.StatModifierType.Add,1),
                new AttributeStats.StatModifier(AttributeStats.StatType.Strength,AttributeStats.StatModifierType.Add,1),
            };

        }
      
    }
}
