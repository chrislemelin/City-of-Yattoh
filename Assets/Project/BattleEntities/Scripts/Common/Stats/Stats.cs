using System;
using System.Collections;
using System.Collections.Generic;
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
        public delegate void UpdateState(object sender);
        public event UpdateState updateStatHandler;
        private List<StatModifier> modifiers = new List<StatModifier>();

        /// <summary>
        /// these are the stats that can change without modifiers, things like health and movement points
        /// </summary>
        private static HashSet<StatType> mutableStatSet = new HashSet<StatType>() { StatType.Health, StatType.Movement, StatType.AP };
        public static HashSet<StatType> MutableStats
        {
            get { return mutableStatSet; }
        }

        [SerializeField]
        private StatInstance baseStats;
        public StatInstance BaseStats
        {
            get { return baseStats; }
        }

        public void Start()
        {
            baseStats.UseDefaults();
            foreach (StatType type in mutableStatSet)
            {
                mutableStats[type] = new Stat(GetStatInstance().GetStat(type));
            }
            mutableStats[StatType.AP] = new Stat(GetStatInstance().GetStat(StatType.AP), 0);

            //smodifiers.Add(new StatModifier(StatType.Movement, StatModifierType.Mult, 1.5f));

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

        private Dictionary<StatType, Stat> mutableStats = new Dictionary<StatType, Stat>();

        public void NewTurn()
        {
            SetMutableStat(StatType.Movement, GetStatInstance().GetStat(StatType.Movement).Value);
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

        public void SetMutableStat(StatType type, int value)
        {
            if (mutableStats.ContainsKey(type))
            {
                mutableStats[type] = new Stat(mutableStats[type], value);
                if (updateStatHandler != null)
                {
                    updateStatHandler(this);
                }
            }
    
        }

        public void SubtractMovementPoints(int value)
        {
            // if its less than zero something should probably happen here
            int newValue = GetMutableStat(StatType.Movement).Value - value;
            SetMutableStat(StatType.Movement, newValue);
        }

        public string StatToString(StatType type)
        {
            if(type == StatType.Movement || type == StatType.Health || type == StatType.AP)
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
            return (StatTypeToString(stat.Type) + ":" + stat.Value);
        }

        private string MutableStatString(Stat stat, Stat maxStat)
        {
            return (StatTypeToString(stat.Type) + ":" + stat.Value + "/" + maxStat.Value);
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