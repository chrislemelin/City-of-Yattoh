using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffRaged : Buff
    {
        private CharacterBoardEntity ragedBy;

        public BuffRaged(CharacterBoardEntity ragedBy, int stacks = 1): base(stacks)
        {
            this.ragedBy = ragedBy;
            description = "Raged by " + ragedBy.Name;
        }

        public override CharacterBoardEntity GetRagedBy(CharacterBoardEntity characterBoardEntity)
        {
            return ragedBy;
        }

    }
}
