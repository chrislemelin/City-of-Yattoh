﻿using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using System;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillBongani2 : Skill
    {

        public SkillBongani2():base()
        {
            description = "lays down a trap which interupts movement";
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
                character.AddPassive(new BuffBleed(2));
                //character.InteruptMovment();
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