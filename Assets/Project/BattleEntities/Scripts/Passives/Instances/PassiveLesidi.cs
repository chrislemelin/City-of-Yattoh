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
            title = "Righteous Blows";
            description = "Attacks deal aditional pure damage";
        }

        public override List<DamagePackage> GetDamagePackage(Skill skill)
        {
            return new List<DamagePackage> () { new DamagePackage(3, DamageType.pure)};
        }


    }
}
