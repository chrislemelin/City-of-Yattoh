using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassiveAreaOfInfluenceTaunt : Passive {

        private HashSet<Tile> influenceTiles = new HashSet<Tile>();
        public List<BuffTaunt> taunts = new List<BuffTaunt>();

        public PassiveAreaOfInfluenceTaunt() : base()
        {
        }

        public override void Init(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager)
        {
            base.Init(battleCalculator, boardEntity, tileManager);
            EnterTile(boardEntity.GetTile());
        }

        private void EnterAction(BoardEntity boardEntity, Tile leavingTile, Action callback)
        {
            if (this.boardEntity.Team != boardEntity.Team && !influenceTiles.Contains(leavingTile))
            { 
                BuffTaunt buff = new BuffTaunt((CharacterBoardEntity)boardEntity);
                taunts.Add(buff);
                ((CharacterBoardEntity)boardEntity).AddPassive(buff);      
            }
            callback();
        }

        public override void EnterTile(Tile tile)
        {
            foreach (Tile t in tileManager.GetTilesDiag(tile.Position))
            {
                influenceTiles.Add(t);
                t.AddEnterActions(EnterAction);
            }
        }

        public override void LeaveTile(Tile tile)
        {
            foreach(BuffTaunt taunt in taunts)
            {
                taunt.PopAll();
            }
            foreach (Tile t in influenceTiles)
            {
                t.RemoveEnterAction(EnterAction);
            }
            influenceTiles.Clear();
        }
        

    }
}
