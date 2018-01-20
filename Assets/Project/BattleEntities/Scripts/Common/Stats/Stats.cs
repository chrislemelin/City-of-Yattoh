using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// this is the master 'stat' class to be used for by other classes
/// </summary>
namespace Placeholdernamespace.Battle.Entities.AttributeStats
{
    [System.Serializable]
    public class Stats
    {
        public delegate void UpdateState();
        public event UpdateState updateStatHandler;
        private List<StatModifier> modifiers = new List<StatModifier>();
        private BoardEntity boardEntity;
        public BoardEntity BoardEntity
        {
            get { return boardEntity; }
        }

        /// <summary>
        /// these are the stats that can change without modifiers, things like health and movement points
        /// </summary>
        private static HashSet<StatType> mutableStatSet = new HashSet<StatType>() { StatType.Health, StatType.Movement, StatType.AP };
        public static HashSet<StatType> MutableStatSet
        {
            get { return mutableStatSet; }
        }

        private Dictionary<StatType, Stat> mutableStats = new Dictionary<StatType, Stat>();
        public Dictionary<StatType, Stat> MutableStats
        {
            get
            {
                return mutableStats.ToDictionary(entry => entry.Key, entry =>  new Stat (entry.Value));
            }
        }


        [SerializeField]
        private StatInstance baseStats;
        public StatInstance BaseStats
        {
            get { return baseStats; }
        }

        public void Start(BoardEntity boardEntity)
        {
            this.boardEntity = boardEntity;
            baseStats.UseDefaults();
            foreach (StatType type in mutableStatSet)
            {
                mutableStats[type] = new Stat(GetStatInstance().GetStat(type), 0);
            }
            mutableStats[StatType.Health] = new Stat(GetStatInstance().GetStat(StatType.Health));

            //modifiers.Add(new StatModifier(StatType.Movement, StatModifierType.Mult, 1.5f));

        }

        public StatInstance GetStatInstance()
        {
            StatInstance returnStats = new StatInstance(baseStats);
            foreach (StatModifier modifier in modifiers)
            {
                returnStats.ApplyMod(modifier);
            }
            return returnStats;

        }

        public Stats GetCopy()
        {
            // a clone for the different previews,
            Stats stats = new Stats();
            stats.modifiers = new List<StatModifier>();
            foreach(StatModifier mod in modifiers)
            {
                stats.modifiers.Add(mod);
            }
            stats.baseStats = baseStats;
            stats.mutableStats = MutableStats;
            stats.boardEntity = boardEntity;
            return stats;
        }

        public void SetStats(Stats stats)
        {
            modifiers = stats.modifiers;
            baseStats = stats.baseStats;
            mutableStats = stats.mutableStats;

        }

        public void NewTurn()
        {
            SetMutableStat(StatType.Movement, 0);
            int currentAP = GetMutableStat(StatType.AP).Value;
            int newAP = currentAP + GetStatInstance().getValue(StatType.APGain);
            SetMutableStat(StatType.AP, newAP);
        }

        public Stat GetMutableStat(StatType type)
        {
            if(mutableStats.ContainsKey(type))
            {
                return mutableStats[type];
            }
            else
            {
                return null;
            }
        }

        public Stat GetNonMuttableStat(StatType type)
        {
            return GetStatInstance().GetStat(type);
        }

        /// <summary>
        /// get mutable stat if it is mutable, else gets the non mutable stat
        /// </summary>
        /// <returns></returns>
        public Stat GetDefaultStat(StatType type)
        {
            if(mutableStats.ContainsKey(type))
            {
                return GetMutableStat(type);
            }
            return GetNonMuttableStat(type);
        }

        public void SetMutableStat(StatType type, int value)
        {
            if (mutableStats.ContainsKey(type))
            {
                if(value < 0)
                {
                    value = 0;
                }
                if(value > GetNonMuttableStat(type).Value)
                {
                    value = GetNonMuttableStat(type).Value;
                }
                mutableStats[type] = new Stat(mutableStats[type], value);
                if (updateStatHandler != null)
                {
                    updateStatHandler();
                }
            }
    
        }

        public void AddActionPoints(int value)
        {
            // if its less than zero something should probably happen here
            int newValue = GetMutableStat(StatType.AP).Value + value;
            SetMutableStat(StatType.AP, newValue);
        }

        public void SubtractMovementPoints(int value)
        {
            // if its less than zero something should probably happen here
            int newValue = GetMutableStat(StatType.Movement).Value - value;
            SetMutableStat(StatType.Movement, newValue);
        }

        public void SubtractAPPoints(int value)
        {
            // if its less than zero something should probably happen here
            int newValue = GetMutableStat(StatType.AP).Value - value;
            SetMutableStat(StatType.AP, newValue);
        }

        public void SubtractHealthPoints(int value)
        {
            // if its less than zero something should probably happen here
            int newValue = GetMutableStat(StatType.Health).Value - value;
            SetMutableStat(StatType.Health, newValue);
        }


        public string StatToString(StatType type)
        {
            return StatTypeToString(type) + ":" + StatValueString(type);            
        }

        public string StatValueString(StatType type)
        {
            if(mutableStats.ContainsKey(type))
            {
                return MutableStatString(GetMutableStat(type), GetStatInstance().GetStat(type));
            }
            else
            {
                return StatString(GetStatInstance().GetStat(type));
            }
        }

        private string StatString(Stat stat)
        {
            return (stat.Value+"");
        }

        private string MutableStatString(Stat stat, Stat maxStat)
        {
            return (stat.Value + "/" + maxStat.Value);
        }

        public static String StatTypeToString(StatType type)
        {
            switch(type)
            {
                case StatType.AP:
                    return "AP";
                case StatType.APGain:
                    return "AP Gain";
                case StatType.Armour:
                    return "Armour";
                case StatType.Health:
                    return "Health";
                case StatType.Inteligence:
                    return "Inteligence";
                case StatType.Movement:
                    return "Movement";
                case StatType.Speed:
                    return "Speed";
                case StatType.Strength:
                    return "Strength";
            }
            return "";
        }
    }

    public enum StatType { Movement, Health, Armour, Strength, Inteligence, Speed, AP, APGain }

}