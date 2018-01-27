using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Instances
{
    public class CharacterBoardEntityJaz : CharacterBoardEntity
    {

        public override void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, 
            BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            base.Init(startingPosition, turnManager, tileManager, boardEntitySelector, battleCalculator);
            range = 4;

            //AddPassive(new PassiveAreaOfInfluenceTaunt());
        }

    }
}

