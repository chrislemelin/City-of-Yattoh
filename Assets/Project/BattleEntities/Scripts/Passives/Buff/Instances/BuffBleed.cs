using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class BuffBleed : Buff
    {
        public BuffBleed(int stacks): base(stacks)
        {
            description = "take a point of damage on movement";
            type = PassiveType.Debuff;
        }

        public override void EnterTile(Tile t)
        {
            DamagePackage package = new DamagePackage(1,DamageType.pure);
            battleCalculator.QuickDamage(boardEntity, new List<DamagePackage>() { package });
        }
    }
}
