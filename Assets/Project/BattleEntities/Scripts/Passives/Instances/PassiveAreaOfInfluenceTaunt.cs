using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassiveAreaOfInfluenceTaunt : Passive  {
        private HashSet<Tile> influenceTiles = new HashSet<Tile>();

        public PassiveAreaOfInfluenceTaunt(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager): 
            base(battleCalculator, boardEntity, tileManager)
        {
            EnterTile(boardEntity.GetTile());

        }

        public override void EnterTile(Tile tile)
        {
            foreach (Tile t in tileManager.GetTilesDiag(tile.Position))
            {
                influenceTiles.Add(t);               
            }
        }

        public override void LeaveTile(Tile tile)
        {
            influenceTiles.Clear();
        }

        public override HashSet<Tile> GetTauntTiles()
        {
            return influenceTiles;
        }
        

    }
}
