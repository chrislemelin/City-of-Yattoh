using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class BuffStun : Buff
    {

        public BuffStun(int stacks = 1):base(stacks)
        {
            type = PassiveType.Debuff;
            skip = true;
            description = "Skips next " + stacks + " turn(s)";
        }
        
    }
}
