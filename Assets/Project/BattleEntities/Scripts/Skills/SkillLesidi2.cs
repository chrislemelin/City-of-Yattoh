﻿using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillLesidi2 : Skill
    {
        public SkillLesidi2() :base()
        {
            title = "Guiding Light";
            description = "An adjacent ally's stats are increased by one for two turns";
            animationType = Common.Animator.AnimatorUtils.animationType.skill;
            apCost = 2;
            coolDown = 4;
            range = 2;
            targetSelfTeam = true;
            buffs.Add(new BuffLesidiAbility2());
        }

        public override List<Tile> TileSetHelper(Position p)
        {
            List<Tile> tiles = base.TileSetHelper(p);
            tiles.Add(boardEntity.GetTile());
            return tiles;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        {
            SkillReport report = new SkillReport() {
                targetAfter = t[0].BoardEntity.Stats.GetCopy(),
                targetBefore = t[0].BoardEntity.Stats.GetCopy() };
            report.Buffs = GetBuffs();
            return report;
            
        }


    }
}
