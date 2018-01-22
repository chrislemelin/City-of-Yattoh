using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassiveAreaOfInfluence : Passive
    {
        private Skill influenceSkill;
        private List<Tile> influenceTiles = new List<Tile>();

        public PassiveAreaOfInfluence(BattleCalculator battleCalculator, BoardEntity boardEntity): base(battleCalculator, boardEntity)
        {
            if(boardEntity is CharacterBoardEntity)
            {
                influenceSkill = ((CharacterBoardEntity)boardEntity).BasicAttack;
            }
        }

        public override void EnterTile(Tile tile)
        {
            foreach(Tile t in influenceSkill.TileSetPossible(tile.Position))
            {
                influenceTiles.Add(t);
                t.AddEnterActions(EnterAction);
            }
        }

        public override void LeaveTile(Tile tile)
        {
            foreach(Tile t in influenceTiles)
            {
                t.RemoveEnterAction(EnterAction);
            }
            influenceTiles.Clear();
        }

        private void EnterAction (BoardEntity boardEntity, Tile leavingTile, Action callback)
        {
            if (this.boardEntity.Team != boardEntity.Team && !influenceTiles.Contains(leavingTile))
                influenceSkill.Action(boardEntity.GetTile(), callback);
            else
                callback();
        }



        
      
    }
}
