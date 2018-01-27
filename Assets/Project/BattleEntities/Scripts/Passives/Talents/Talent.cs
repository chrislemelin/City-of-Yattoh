using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public abstract class Talent : Passive
    {
        public Talent(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager) :
            base(battleCalculator, boardEntity, tileManager)
        {
            type = PassiveType.Talent;
        }

        public abstract void Activate();
        
    }
}
