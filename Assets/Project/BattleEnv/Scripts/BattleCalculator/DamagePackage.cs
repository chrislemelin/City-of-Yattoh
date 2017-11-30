using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Calculator
{
    public class DamagePackage : MonoBehaviour {

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
    }
}