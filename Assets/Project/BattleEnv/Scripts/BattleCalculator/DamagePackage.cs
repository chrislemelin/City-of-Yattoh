using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class DamagePackage {

        private int damage;
        public int Damage
        {
            get { return damage; }
        }

        private DamageType type;
        public DamageType Type
        {
            get { return type; }
        }   

        public DamagePackage(int damage, DamageType type)
        {
            this.damage = damage;
            this.type = type;
        }

        public DamagePackage(DamagePackageInternal internalpackage)
        {
            damage = (int)internalpackage.damage;
            type = internalpackage.type;
        }
    }
}