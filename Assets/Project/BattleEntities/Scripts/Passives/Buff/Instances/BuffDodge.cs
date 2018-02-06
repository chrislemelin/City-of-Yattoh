using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffDodge : Buff
    {

        public BuffDodge(int stacks): base(stacks)
        {
            popOneAtTurnEnd = false;
        }

        public override string GetDescription()
        {
            return "avoid next " + stacks + " attack(s)";
        }

        public override TakeDamageReturn TakeDamage(Skill skill, DamagePackage package, TakeDamageReturn lastReturn)
        {
            return new TakeDamageReturn() { type = TakeDamageReturnType.NoDamage, value = 1f };
        }

        public override void AttackedBy(CharacterBoardEntity character)
        {
            PopStack();
        }

    }
}