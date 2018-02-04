using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using System;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillBongani2 : Skill
    {

        public SkillBongani2():base()
        {
            description = "lays down a trap which applies a -1 strength debuff";
            title = "Lay Trap";
            apCost = 2;
            coolDown = 3;
            range = 3;
        }

        protected override SkillReport ActionHelper(List<Tile> t)
        {
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action<bool> callback)
        {
            if(tiles.Count > 0)
            {
                tileManager.AddTrap(tiles[0], EnterTile);
            }
        }

        private bool EnterTile(Tile t, CharacterBoardEntity character)
        {
            if(character.Team == OtherTeam())
            {
                character.Stats.SetMutableStat(AttributeStats.StatType.Movement, 0);
                character.InteruptMovment();
                return true;

            }
            return false;
        }

        protected override bool TileHasTarget(Tile t)
        {
            return true;
        }

    }
}
