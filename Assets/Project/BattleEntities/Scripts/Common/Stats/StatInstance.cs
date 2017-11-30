using Placeholdernamespace.Battle.Entities.AttributeStats.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.AttributeStats
{
    [System.Serializable]
    public class StatInstance
    {
        [SerializeField]
        private StatInternal armour;

        [SerializeField]
        private StatInternal inteligence;

        [SerializeField]
        private StatInternal maxHealth;

        [SerializeField]
        private StatInternal movement;

        [SerializeField]
        private StatInternal speed;

        [SerializeField]
        private StatInternal strength;

        [SerializeField]
        private StatInternal ap;

        [SerializeField]
        private StatInternal apGain;

        private Dictionary<StatType, StatInternal> stats = new Dictionary<StatType, StatInternal>();

        public StatInstance()
        {
            stats = new Dictionary<StatType, StatInternal>();
        }

        public StatInstance(StatInstance obj)
        {
            stats.Clear();
            foreach (StatType type in obj.stats.Keys)
            {
                StatInternal stat = obj.stats[type];
                stats.Add(type, new StatInternal(stat.Type, stat.Value));
            }
        }

        public void UseDefaults()
        {
            stats.Clear();
            
            stats.Add(StatType.Armour,  new StatInternal(StatType.Armour,armour));
            stats.Add(StatType.Inteligence, new StatInternal(StatType.Inteligence, inteligence));
            stats.Add(StatType.Health, new StatInternal(StatType.Health, maxHealth));
            stats.Add(StatType.Movement, new StatInternal(StatType.Movement, movement));
            stats.Add(StatType.Speed, new StatInternal(StatType.Speed, speed));
            stats.Add(StatType.Strength, new StatInternal(StatType.Strength, strength));
            stats.Add(StatType.AP, new StatInternal(StatType.AP, ap));
            stats.Add(StatType.APGain, new StatInternal(StatType.APGain, apGain));
            
        }

        public int getValue(StatType type)
        {
            if (stats.ContainsKey(type))
            {
                return (int)stats[type].Value;
            }
            return -1;
        }

        public void ApplyMod(StatModifier mod)
        {
            if (stats.ContainsKey(mod.StatType))
            {
                StatInternal newStat = new StatInternal(stats[mod.StatType].Type, mod.Apply(stats[mod.StatType].Value, mod.StatType));
                stats.Remove(mod.StatType);
                stats.Add(mod.StatType, newStat);
            }
            else
            {
                Console.WriteLine("doesnt have stat of type " + mod.StatType);
            }
        }

        public Stat GetStat(StatType type)
        {
            if (stats.ContainsKey(type))
            {
                Stat returnStat = new Stat(stats[type]);              
                return returnStat;
            }
            return null;
        }

        public IEnumerable<Stat> GetStats()
        {
            List<Stat> returnStats = new List<Stat>();
            foreach(StatInternal stat in stats.Values)
            {
                returnStats.Add(new Stat(stat));
            }
            return returnStats;
        }
    }
}