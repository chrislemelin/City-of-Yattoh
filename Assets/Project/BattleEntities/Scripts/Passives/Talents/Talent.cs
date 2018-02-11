using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public abstract class Talent : Passive
    {
        public Talent() :base()
        {
            type = PassiveType.Talent;

        }

        public override string GetTitle()
        {
            return title + " (TALENT)";
        }

        public abstract void Activate();
        
    }
}
