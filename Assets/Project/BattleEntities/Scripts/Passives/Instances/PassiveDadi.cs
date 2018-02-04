using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class PassiveDadi : Passive
    {
        public PassiveDadi(): base()
        {
            description = "gain extra ap each turn equal to number of adjacent enemies divided by 2";
        }

        public override void StartTurn()
        {
            List<Tile> tiles = tileManager.GetTilesDiag(boardEntity.Position);
            List <CharacterBoardEntity> chars = tileManager.TilesToCharacterBoardEntities(tiles);
            int counter = 0;
            foreach(CharacterBoardEntity character in chars)
            {
                if(character.Team != boardEntity.Team)
                {
                    counter++;
                }
            }
            boardEntity.Stats.AddActionPoints(counter / 2);
        }

    }
}
