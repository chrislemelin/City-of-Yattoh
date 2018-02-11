using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentBongani2 : Talent
    {
        public TalentBongani2()
        {
            title = "weaken";
            description = "Weaken adjacent enemies";
        }

        public override void Activate()
        {
         
            List<Tile> tiles = tileManager.GetTilesDiag(boardEntity.GetTile().Position, 1);

            foreach(CharacterBoardEntity character in tileManager.TilesToCharacterBoardEntities(tiles))
            {
                if(character.Team != boardEntity.Team)
                {
                    character.AddPassive(new BuffWeaken(1, 2));
                }
            }
        }
    }
}
