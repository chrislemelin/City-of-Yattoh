using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class BuffStealth : Buff
    {
        public BuffStealth(int stacks): base(stacks)
        {
            stealthed = true;
            description = "Cant be targeted by enemies";
        }
       
    }
}
