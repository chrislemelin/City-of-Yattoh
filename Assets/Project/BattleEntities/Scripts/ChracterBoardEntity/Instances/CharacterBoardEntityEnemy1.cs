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

    public class CharacterBoardEntityEnemy1 : CharacterBoardEntity
    {

        public override void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            base.Init(startingPosition, turnManager, tileManager, boardEntitySelector, battleCalculator);

            //CharacterBoardEntityJaz doYouLikeJaz = FindObjectOfType<CharacterBoardEntityJaz>();
            //Passive passive = new BuffRaged(doYouLikeJaz, 4);
            //AddPassive(passive);
           
        }

    }
}
