using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.Battle.Entities.Instances
{
    public class CharacterBoardEntityJaz : CharacterBoardEntity
    {

        public override void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, 
            BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            base.Init(startingPosition, turnManager, tileManager, boardEntitySelector, battleCalculator);
            range = 4;

            AddPassive(new PassivePiercingJaz());
            AddPassive(new TalentJaz());
            AddPassive(new TalentTriggerJaz());

            AddSkill(new SkillJaz1());
            AddSkill(new SkillJaz2());
            //AddPassive(new )
            //AddPassive(new PassiveAreaOfInfluenceTaunt());
        }

    }
}

