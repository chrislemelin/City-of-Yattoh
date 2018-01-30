using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class PassiveLesidi : Passive
    {

        public PassiveLesidi():base()
        {
            description = "Adds 3 pure damage on attacks";
        }

        public override List<DamagePackage> GetDamagePackage(Skill skill)
        {
            return new List<DamagePackage> () { new DamagePackage(3, DamageType.pure)};
        }


    }
}
