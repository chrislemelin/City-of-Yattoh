using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities
{ 
    public class EnemyBoardEntity : BoardEntity {

        public override void Init(TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector)
        {
            base.Init(turnManager, tileManager, boardEntitySelector);
        }

        public override void StartMyTurn()
        {
        }
    }
}
