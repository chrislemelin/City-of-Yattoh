using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Common.Animator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Calculator;
using System;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class BasicAttack : Skill
    {

        public BasicAttack():base()
        {
            title = "Basic Attack";
            description = "Deal STRENGTH damage to one enemy";
            apCost = 1;
            coolDown = 1;
            piercing = 0;
            flavorText = "this is the basic attack";
        }

        protected override int? GetRangeInternal()
        {
            return boardEntity.Range;
        }

        protected override int? GetStrengthInternal()
        {
            return boardEntity.Stats.GetNonMuttableStat(AttributeStats.StatType.Strength).Value;
        }

        protected override SkillReport ActionHelper(List<Tile> tiles)
        {
            List<DamagePackage> packages = GenerateDamagePackages();
            List<BoardEntity> entities =  TeamTiles(tiles, OtherTeam());

            SkillReport report = null;

            foreach(BoardEntity entity in entities)
            {
                if(entity.Team != boardEntity.Team)
                    report = battleCalculator.ExecuteSkillDamage(boardEntity, this, (CharacterBoardEntity)entity,
                        packages);

            }
            return report;
        }
    }
}
