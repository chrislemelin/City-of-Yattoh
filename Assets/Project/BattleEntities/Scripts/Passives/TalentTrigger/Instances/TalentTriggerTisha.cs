using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class TalentTriggerTisha : TalentTrigger
    {

        public TalentTriggerTisha(): base()
        {
            title = "Overwhelming Odds";
            description = "When you end your turn adjacent to 2 or more enemies TRIGGER";
        }

        public override void EndTurn()
        {
            List<CharacterBoardEntity> enemies = tileManager.TilesToCharacterBoardEntities(tileManager.GetTilesDiag(boardEntity.Position, 1),boardEntity.Team);
            if(enemies.Count >= 2)
            {
                Trigger();
            }
        }

    }
}
