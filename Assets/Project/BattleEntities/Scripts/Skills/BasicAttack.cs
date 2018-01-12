using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Common.Animator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Calculator;
using System;
using Placeholdernamespace.Battle.Interaction;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class BasicAttack : Skill
    {
        public BasicAttack()
        {
            title = "Basic Attack";
            APCost = 1;
        }

        protected override List<Tile> TileSetHelper(Position p)
        {
            return TeamTiles(tileManager.GetAllAdjacentTiles(p), boardEntity.Team);
        }

        public override List<TileSelectOption> TileOptionSet()
        {
            List<Tile> tiles = TileSetHelper(boardEntity.Position);
            List<TileSelectOption> tileOptions = new List<TileSelectOption>();           
            foreach(Tile t in tiles)
            {
                SkillReport report = GetSkillReport(t);
                tileOptions.Add(new TileSelectOption
                {
                    Selection = t,
                    OnHover = new List<Tile>() { t },
                    HighlightColor = selectColor,
                    HoverColor = highlightColor,
                    DisplayStats = report.TargetAfter,
                });
            }
            return tileOptions;
        }

        public override void Action(Tile t, Action callback = null)
        {
            DamagePackageInternal damagePackage = GenerateDamagePackage();
            DamagePackage package = new DamagePackage(damagePackage);

            battleCalculator.DoDamage(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, package);
            boardEntity.Stats.SubtractAPPoints(GetAPCost());
            boardEntity.GetComponentInChildren<Animator>().SetInteger("Attack", AnimatorUtils.GetAttackDirectionCode(boardEntity.Position, t.Position));
            if (callback != null)
            {
                callback();
            }
        }   

        private SkillReport GetSkillReport(Tile t)
        {
            DamagePackageInternal damagePackage = GenerateDamagePackage();
            DamagePackage package = new DamagePackage(damagePackage);
            return battleCalculator.ExecuteSkillHelper(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, package);
        }

        protected DamagePackageInternal GenerateDamagePackage()
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
