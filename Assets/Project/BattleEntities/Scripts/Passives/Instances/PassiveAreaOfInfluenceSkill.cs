using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassiveAreaOfInfluenceSkill : Passive
    {
        private Skill influenceSkill;
        private List<Tile> influenceTiles = new List<Tile>();

        public PassiveAreaOfInfluenceSkill(): base()
        {            
        }

        public override void Init(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager)
        {
            base.Init(battleCalculator, boardEntity, tileManager);
            influenceSkill = boardEntity.BasicAttack;
            EnterTile(boardEntity.GetTile());

        }

        public override void EnterTile(Tile tile)
        {
            foreach(Tile t in influenceSkill.TileSetHelper(tile.Position))
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
                influenceSkill.Action(boardEntity.GetTile(),  callback);
            else
                callback();
        }
     
    }
}
