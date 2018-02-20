using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class PassiveBongani : Passive
    {

        public PassiveBongani():base()
        {
            turnOrderFirst = true;
            title = "Well-Laid Plans";
            description = "Always go first in the turn order";
        }       

    }
}
