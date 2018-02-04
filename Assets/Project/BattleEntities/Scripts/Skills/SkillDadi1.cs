using System;
using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Common.Animator;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillDadi1 : Skill
    {
        List<Tile> upTiles, downTiles, rightTiles, leftTiles;

        public SkillDadi1(): base()
        {
            animationType = AnimatorUtils.animationType.none;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        {
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action<bool> callback)
        {
            int range = boardEntity.Stats.GetNonMuttableStat(AttributeStats.StatType.Movement).Value;
            Position startTile = boardEntity.GetTile().Position;
            Move move = new Move() { apCost = 0, movementCost = 0, path = tiles, destination = tiles[tiles.Count-1]};
            Position direction = tiles[0].Position - boardEntity.GetTile().Position;
            boardEntity.ExecuteCharge(move, direction, callback);

        }

        protected override void DoCallback(Action<bool> callback = null){}

        public override bool TileOptionClickable(Tile t)
        {
            return true;
        }

        public override List<Tile> TileReturnHelper(Tile t)
        {
            return GetTileListHelper(t);
        }

        private List<Tile> GetTileListHelper(Tile t)
        {
            if (upTiles.Contains(t))
                return upTiles;
            if (downTiles.Contains(t))
                return downTiles;
            if (rightTiles.Contains(t))
                return rightTiles;
            if (leftTiles.Contains(t))
                return leftTiles;
            return null;
        }

        public override List<Tile> TileSetHelper(Position p)
        {
            int range = boardEntity.Stats.GetNonMuttableStat(AttributeStats.StatType.Movement).Value;
            Position startTile = boardEntity.GetTile().Position;

            upTiles = ChargeListHelper(tileManager.GetTilesInDirection(startTile, new Position(0, 1), range));
            downTiles = ChargeListHelper(tileManager.GetTilesInDirection(startTile, new Position(0, -1), range));
            rightTiles = ChargeListHelper(tileManager.GetTilesInDirection(startTile, new Position(1, 0), range));
            leftTiles = ChargeListHelper(tileManager.GetTilesInDirection(startTile, new Position(-1, 0), range));

            

            List<Tile> returnTiles = new List<Tile>();
            returnTiles.AddRange(upTiles);
            returnTiles.AddRange(downTiles);
            returnTiles.AddRange(leftTiles);
            returnTiles.AddRange(rightTiles);

            return returnTiles;
        }

        private List<Tile> ChargeListHelper(List<Tile> tiles)
        {
            List<Tile> returnTiles = new List<Tile>();
            bool hitEnemy = false;
            foreach (Tile tile in tiles)
            {
                if (tile.BoardEntity != null)
                {
                    if (tile.BoardEntity.Team == boardEntity.Team && hitEnemy)
                    {
                        break;
                    }
                    if(tile.BoardEntity.Team != boardEntity.Team)
                    {
                        if(hitEnemy)
                        {
                            returnTiles.RemoveAt(returnTiles.Count - 1);
                            break;
                            
                        }
                        else
                        {
                            hitEnemy = true;
                        }
                    }
                }
                returnTiles.Add(tile);
            }


            return returnTiles;
        }

        
    }
}
