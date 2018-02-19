using System;
using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillAmare1 : Skill
    {

        public SkillAmare1(): base()
        {
            title = "Desperate Flurry";
            description = "Exhast all energy to unleash repeated attacks";
            apCost = 0;
            coolDown = 2;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        {
            int ap = boardEntity.Stats.GetMutableStat(AttributeStats.StatType.AP).Value;
            DamagePackage package = boardEntity.BasicAttack.GenerateDamagePackage();
            DamagePackage newPackage = new DamagePackage(package.Damage / 2, package.Type, package.Piercing);
            List<DamagePackage> packs = new List<DamagePackage>();

            for (int a = 0 ; a < ap;a++)
            {
                packs.Add(newPackage);
                packs.Add(newPackage);
            }
            return battleCalculator.ExecuteSkillDamage(boardEntity, this, ((CharacterBoardEntity)t[0].BoardEntity), packs);
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action<bool> calback = null)
        {
            boardEntity.Stats.SetMutableStat(AttributeStats.StatType.AP, 0);
        }

    }
}
