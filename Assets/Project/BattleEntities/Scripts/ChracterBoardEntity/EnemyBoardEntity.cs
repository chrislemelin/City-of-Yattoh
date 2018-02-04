using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.Battle.Entities
{ 
    public class EnemyBoardEntity : BoardEntity {
        public override void AddPassive(Passive passive)
        {
            throw new NotImplementedException();
        }

        public override void AddSkill(Skill skill)
        {
            throw new NotImplementedException();
        }

        public override void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            base.Init(startingPosition, turnManager, tileManager, boardEntitySelector, battleCalculator);
        }

        public override void StartMyTurn()
        {
        }
    }
}
