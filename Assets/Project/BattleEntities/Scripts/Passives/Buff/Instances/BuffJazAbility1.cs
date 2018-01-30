using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.AttributeStats;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class BuffJazAbility1 : Buff
    {

        public BuffJazAbility1(int stacks):base()
        {
            
            description = "adds +1 power per stack. Lose a stack on movement";
            this.stacks = stacks;
        }

        public override void EnterTile(Tile t)
        {
            PopStack(1);
        }

        protected override List<StatModifier> GetStatHelperModifiers()
        {
            return new List<StatModifier>() { new StatModifier(StatType.Strength, StatModifierType.Add, stacks) };
        }
    }
}
