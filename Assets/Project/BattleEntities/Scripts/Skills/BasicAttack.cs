﻿using Placeholdernamespace.Battle.Env;
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

        public BasicAttack(TileManager tileManager, CharacterBoardEntity boardEntity, BattleCalculator battleCalculator):base(tileManager,boardEntity,battleCalculator)
        {
            title = "Basic Attack";
            description = "Deal STRENGTH damage to one enemy";
            apCost = 1;
            coolDown = 1;
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

        protected override void ActionHelper(Tile t)
        {
            DamagePackageInternal damagePackage = GenerateDamagePackage();
            DamagePackage package = new DamagePackage(damagePackage);

            battleCalculator.ExecuteSkillDamage(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, package);
        }
    }
}
