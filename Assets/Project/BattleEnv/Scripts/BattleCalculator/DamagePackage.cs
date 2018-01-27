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

        private int piercing;
        public int Piercing
        {
            get { return piercing; }
        }

        private DamageType type;
        public DamageType Type
        {
            get { return type; }
        }   

        public DamagePackage(int damage, DamageType type, int piercing = 0)
        {
            this.damage = damage;
            this.type = type;
            this.piercing = piercing;
        }

        public DamagePackage(DamagePackageInternal internalpackage)
        {
            damage = (int)internalpackage.Damage;
            type = internalpackage.Type;
            piercing = (int)internalpackage.Piercing;
        }
    }
}