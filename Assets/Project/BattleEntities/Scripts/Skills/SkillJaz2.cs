﻿using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Env;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Skills
{

    public class SkillJaz2 : Skill
    {

        List<Tile> upTiles, downTiles, rightTiles, leftTiles;

        public SkillJaz2():base()
        {
            title = "Piercing Shot";
            apCost = 2;
            coolDown = 3;
            description = "Deal half of power as damage and armour damage to enemies a single direction, has range equal to basic attack";
        }

        protected override SkillReport GetSkillReport(Tile t)
        {
            return null;
        }

        public override List<Tile> TileSetHelper(Position p)
        {
            int range = (int)boardEntity.Range;
            if (range == RANGE_ADJACENT)
                range = 1;
            Position startTile = boardEntity.GetTile().Position;

            upTiles = tileManager.GetTilesInDirection(startTile, new Position(0, 1), range);
            downTiles = tileManager.GetTilesInDirection(startTile, new Position(0, -1), range);
            rightTiles = tileManager.GetTilesInDirection(startTile, new Position(1, 0), range);
            leftTiles = tileManager.GetTilesInDirection(startTile, new Position(-1, 0), range);

            List<Tile> returnTiles = new List<Tile>();
            returnTiles.AddRange(upTiles);
            returnTiles.AddRange(downTiles);
            returnTiles.AddRange(leftTiles);
            returnTiles.AddRange(rightTiles);

            return returnTiles;
        }

        public override bool TileOptionClickable(Tile t)
        {
            List<Tile> tiles = GetTileListHelper(t);
            if(tiles != null)
            {
                foreach(Tile tile in tiles)
                {
                    if( base.TileOptionClickable(tile))
                    {
                        return true;
                    }
                }
            }
            return false;
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

        protected override SkillReport ActionHelper(List<Tile> tiles)
        {
            SkillReport skillReport = null;
            if (tiles != null)
            {
                foreach(Tile t in tiles)
                {
                    if (t.BoardEntity != null && TileHasTarget(t))
                    {
                        skillReport = battleCalculator.ExecuteSkillDamage(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, 
                            GenerateDamagePackages() );
                        /*
                        DamagePackage armourPackage = new DamagePackage(
                            boardEntity.Stats.GetNonMuttableStat(AttributeStats.StatType.Strength).Value / 2,
                            DamageType.armour
                            );
                        battleCalculator.ExecuteSkillDamage(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, armourPackage);
                        */
                    }
                }
            }
            return skillReport;
        }

        public override List<Buff> GetBuffs()
        {
            return new List<Buff>() { new BuffMinusArmour(2, boardEntity.Stats.GetNonMuttableStat(AttributeStats.StatType.Strength).Value / 2) };
        }

        protected override int? GetStrengthInternal()
        {
            return boardEntity.BasicAttack.GetStrength()/2;
        }

        protected override int? GetRangeInternal()
        {
            return boardEntity.Range;
        }

    }
}
