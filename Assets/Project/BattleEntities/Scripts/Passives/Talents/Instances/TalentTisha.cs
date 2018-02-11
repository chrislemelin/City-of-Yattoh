using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentTisha : Talent
    {
        public TalentTisha(): base()
       {
            title = "Challenger";
            description = "TALENT: adjacent enemies cannot leave tiles adjacent to character";
        }

        public override void Activate()
        {
            List<Tile> tiles = tileManager.GetTilesDiag(boardEntity.Position, 1);
            //HashSet<Tile>

            List<CharacterBoardEntity> entities = tileManager.TilesToCharacterBoardEntities(tiles,boardEntity.Team);
            foreach(CharacterBoardEntity character in entities)
            {
                character.AddPassive(new BuffTaunt(boardEntity));
            }

        }
    }
}
