using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentTriggerBongani2 : TalentTrigger
    {
        private List<Tile> influenceTiles = new List<Tile>();

        public TalentTriggerBongani2()
        {
            description = "When enemy enters adjacency TRIGGER";
        }

        public override void Init(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager)
        {
            base.Init(battleCalculator, boardEntity, tileManager);
            EnterTile(boardEntity.GetTile());

        }

        public override void EnterTile(Tile tile)
        {
            foreach (Tile t in tileManager.GetTilesNoDiag(tile.Position))
            {
                influenceTiles.Add(t);
                t.AddEnterActions(EnterAction);
            }
        }

        public override void LeaveTile(Tile tile)
        {
            foreach (Tile t in influenceTiles)
            {
                t.RemoveEnterAction(EnterAction);
            }
            influenceTiles.Clear();
        }

        private void EnterAction(BoardEntity boardEntity, Tile leavingTile, Action callback)
        {
            if (this.boardEntity.Team != boardEntity.Team && !influenceTiles.Contains(leavingTile))
            {
                Trigger();
            }
            callback();
        }
    }
}
