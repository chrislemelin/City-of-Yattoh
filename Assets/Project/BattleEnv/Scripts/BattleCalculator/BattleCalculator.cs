using Placeholdernamespace.Battle.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Calculator
{

    public class BattleCalculator : MonoBehaviour
    {
        public void DoDamage(CharacterBoardEntity source, CharacterBoardEntity target, DamagePackageInternal damage)
        {
            int newHealth = target.Stats.GetMutableStat(Entities.AttributeStats.StatType.Health).Value - (int)damage.damage;
            if(newHealth < 0)
            {
                // dat bibba dead
                newHealth = 0;
            }
            target.Stats.SetMutableStat(Entities.AttributeStats.StatType.Health, newHealth);
        }
        
    }

    public enum DamageType { physical, pure};

    public class DamagePackageInternal
    {
        public float damage;
        public float Damage
        {
            get { return damage; }
        }

        public DamageType type;
        public DamageType Type
        {
            get { return type; }
        }

        public DamagePackageInternal(float damage, DamageType type)
        {
            this.damage = damage;
            this.type = type;
        }
    }
}
