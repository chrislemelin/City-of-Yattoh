using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffWeaken : Buff
    {
        private int value;

        public BuffWeaken(int value, int stacks): base(stacks)
        {
            this.value = value;
            description = "-"+1 +" strength";

        }

        protected override List<StatModifier> GetStatHelperModifiers()
        {
            List<StatModifier> mods = new List<StatModifier>();
            mods.Add(new StatModifier(StatType.Strength, StatModifierType.Add, -value));
            return mods;
        }



    }
}
