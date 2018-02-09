using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffStealth : Buff
    {
        public BuffStealth(int stacks): base(stacks)
        {
            stealthed = true;
            description = "Cant be targeted by enemies";
        }

        public override void ExecutedSkill(Skill skill, SkillReport skillreport)
        {
            Remove();
        }

    }
}
