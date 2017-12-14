using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Common.Animator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Calculator;
using System;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class BasicAttack : Skill
    {
        public BasicAttack()
        {
            title = "Basic Attack";
            APCost = 1;
        }

        public override List<Tile> TileSet()
        {
            return TileSetHelper(boardEntity.GetTile().Position);
        }

        public List<Tile> TheoreticalTileSet(Position p)
        {
            return TileSetHelper(p);
        }

        private List<Tile> TileSetHelper(Position p)
        {
            return TeamTiles(tileManager.GetAllTilesNear(p), boardEntity.Team);
        }

        public override void Action(Tile t, Action callback = null)
        {
            DamagePackageInternal damagePackage = GenerateDamagePackage();
            DamagePackage package = new DamagePackage(damagePackage);

            battleCalculator.DoDamage(boardEntity, (CharacterBoardEntity)t.BoardEntity, package);
            boardEntity.Stats.SubtractAPPoints(APCost);
            boardEntity.GetComponentInChildren<Animator>().SetInteger("Attack", AnimatorUtils.GetAttackDirectionCode(boardEntity.Position, t.Position));
            if (callback != null)
            {
                callback();
            }
        }   

        private DamagePackageInternal GenerateDamagePackage()
        {
            int basePower = boardEntity.Stats.GetStatInstance().getValue(AttributeStats.StatType.Strength);
            Dictionary<SkillModifierType, int> baseStats = new Dictionary<SkillModifierType, int>();
            baseStats[SkillModifierType.Power] = basePower;
            Dictionary<SkillModifierType, int> effectiveStats = getSkillModifiers(baseStats);

            int effectivePower = effectiveStats[SkillModifierType.Power];
            return new DamagePackageInternal(effectivePower, DamageType.physical);
        }

    }
}
