using Placeholdernamespace.Battle.Entities.Passives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentLesidi : Talent
    {
        public TalentLesidi()
        {
            title = "Inspiring Words";
            description = "TALENT reduces cooldown of self and adjacent allies";
        }

        public override void Activate()
        {
            List<BoardEntity> enitites = tileManager.GetBoardEntityDiag(boardEntity.GetTile().Position, 1);
            List<CharacterBoardEntity> chars =  Core.convert(enitites);
            foreach(CharacterBoardEntity character in chars)
            {
                character.ReduceCooldowns();
            }
            boardEntity.ReduceCooldowns();
           
        }

      
    }
}