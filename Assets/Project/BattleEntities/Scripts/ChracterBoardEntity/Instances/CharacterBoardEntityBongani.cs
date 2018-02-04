using Placeholdernamespace.Battle.Entities.Passives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.Battle.Entities
{
    public class CharacterBoardEntityBongani : CharacterBoardEntity
    {

        public CharacterBoardEntityBongani():base()
        {
        }

        public override void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            base.Init(startingPosition, turnManager, tileManager, boardEntitySelector, battleCalculator);
            AddPassive(new PassiveBongani());
            AddPassive(new TalentBongani2());
            AddPassive(new TalentTriggerBongani2());
            AddSkill(new SkillBongani3());
            AddSkill(new SkillBongani2());
        }

    }
}
