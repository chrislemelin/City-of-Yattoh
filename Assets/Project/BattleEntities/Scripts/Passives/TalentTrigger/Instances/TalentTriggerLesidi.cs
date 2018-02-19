using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class TalentTriggerLesidi : TalentTrigger
    {

        public TalentTriggerLesidi()
        {
            title = "Side by Side";
            description = "When you move and end your turn next to an ally, activate talents";
        }

        bool trigger = false;

        public override void StartTurn()
        {
            trigger = false;
        }

        public override void ExecutedMove(Move move)
        {
            trigger = true;
        }

        public override void EndTurn()
        {
            if(trigger)
            {
                List<BoardEntity> boardEntities = tileManager.GetBoardEntityDiag(boardEntity.Position,1);
                foreach(BoardEntity be in boardEntities)
                {
                    if(be.Team == boardEntity.Team)
                    {
                        Trigger();
                        break;
                    }
                }             
            }
            
        }
    }
}
