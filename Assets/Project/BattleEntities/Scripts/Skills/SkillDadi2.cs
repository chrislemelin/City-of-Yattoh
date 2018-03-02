using System;
using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillDadi2 : Skill {

        List<Tile> tiles = new List<Tile>();
        bool clickable = false;

        public SkillDadi2():base()
        {
            title = "Bring it On";
            description = "Taunt enemies, forcing them to attack you";
            apCost = 0;
            coolDown = 4;
            range = 2;
        }

        public override List<Tile> TileSetHelper(Position p)
        {
            tiles = tileManager.GetTilesNoDiag(p, GetRange());
            clickable = false;
            foreach(Tile t in tiles)
            {
                if(TileHasTarget(t))
                {
                    clickable = true;
                }
            }
            return tiles;
        }

        public override bool TileOptionClickable(Tile t)
        {
            return clickable;
        }

        public override List<Tile> TileReturnHelper(Tile t)
        {
            return tiles;
        }



        protected override SkillReport ActionHelper(List<Tile> t)
        {
            return null;
        }

        protected override void ActionHelperNoPreview(List<Tile> tiles, Action callback = null)
        {
            List<CharacterBoardEntity> characters = tileManager.TilesToCharacterBoardEntities(tiles, boardEntity.Team);
            foreach(CharacterBoardEntity character in characters)
            {
                character.AddPassive(new BuffRaged(boardEntity));
            }
            base.ActionHelperNoPreview(tiles, callback);

        }



    }
}
