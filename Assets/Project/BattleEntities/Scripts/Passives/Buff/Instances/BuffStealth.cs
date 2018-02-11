using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffStealth : Buff
    {
        public BuffStealth(int stacks): base(stacks)
        {
            stealthed = true;
            description = "Cant be targeted by enemies and adds +2 strength on next attack. Breaks after doing a skill";
            skillModifiers = new List<SkillModifier>() { new SkillModifier(SkillModifierType.Power,SkillModifierApplication.Add,2) };
        }

        public override void Init(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager)
        {
            base.Init(battleCalculator, boardEntity, tileManager);
            boardEntity.SetAplha(.5f);
        }

        public override void ExecutedSkillFast(Skill skill, SkillReport skillreport)
        {
            if(skill.GetRange() != Skill.RANGE_SELF)
            {
                Remove();
            }
        }

        protected override void RemoveHelper()
        {
            boardEntity.SetAplha(1f);
        }




    }
}
