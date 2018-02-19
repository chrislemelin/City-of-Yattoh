using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillLesidi1 : Skill
    {
        //private int heal;

        public SkillLesidi1():base()
        {
            title = "Lay on Hands";
            description = "An adjacent ally or yourself for half of their max health";
            animationType = Common.Animator.AnimatorUtils.animationType.skill;
            range = 2;
            apCost = 1;
            coolDown = 2;
            targetSelfTeam = true;
        }

        public override List<Tile> TileSetHelper(Position p)
        {
            List<Tile> tiles = base.TileSetHelper(p);
            tiles.Add(boardEntity.GetTile());
            return tiles;
        }


        protected override SkillReport ActionHelper(List<Tile> t)
        {
            List<CharacterBoardEntity> list = Core.convert(tileManager.TilesToBoardEntities(t));
            if(list.Count > 0)
            {
                int heal = list[0].Stats.GetNonMuttableStat(AttributeStats.StatType.Health).Value/2;
                return battleCalculator.ExecuteSkillHealing(this, boardEntity, list[0], heal);
            }
            return null;
        }
    }
}
