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
        public List<StatModifier> modifiers = new List<StatModifier>();
        
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

        public List<StatModifier> GetModifiers()
        {
            List<StatModifier> mods = new List<StatModifier>(modifiers);
            if(boardEntity is CharacterBoardEntity)
            {
                mods.AddRange(((CharacterBoardEntity)boardEntity).GetStatModifiers());
            }
            return mods;
        }

        public void AddModifier(StatModifier modifier)
        {
            modifiers.Add(modifier);
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
            baseStats.UseDefaults();
            StatInstance returnStats = new StatInstance(baseStats);
            foreach (StatModifier modifier in GetModifiers())
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
            stats.updateStatHandler += updateStatHandler;
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
        public Stat GetDefaultStat(StatType type, List<StatModifier> modifiers = null)
        {
            if(modifiers != null)
            {
                this.modifiers = modifiers;
            }
            Stat returnStats;
            if (mutableStats.ContainsKey(type))
            {
                returnStats =  GetMutableStat(type);
            }
            else
            {
                returnStats = GetNonMuttableStat(type);
            }
            if (modifiers != null)
            {
                this.modifiers = new List<StatModifier>();
            }
            return returnStats;
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

        public void UpdateStatHandler()
        {
            if (updateStatHandler != null)
            {
                updateStatHandler();
            }
        }

        public void AddActionPoints(int value)
        {
            // if its less than zero something should probably happen here
            int newValue = GetMutableStat(StatType.AP).Value + value;
            SetMutableStat(StatType.AP, newValue);
        }

        public bool SubtractMovementPoints(int value)
        {
            // if its less than zero something should probably happen here
            int newValue = GetMutableStat(StatType.Movement).Value - value;

            int valueToSubract= 0;
            while(newValue < 0)
            {
                newValue += GetNonMuttableStat(StatType.Movement).Value;
                if(GetMutableStat(StatType.AP).Value == 0)
                {
                    return false;
                }
                else
                {
                    valueToSubract++;
                }
            }
            if(valueToSubract > 0) 
                SubtractAPPoints(valueToSubract, true);
            SetMutableStat(StatType.Movement, newValue);
            return true;

        }

        public void SubtractAPPoints(int value, bool display = false)
        {
            // if its less than zero something should probably happen here
            int newValue = GetMutableStat(StatType.AP).Value - value;
            SetMutableStat(StatType.AP, newValue);
            if(display)
                ((CharacterBoardEntity)boardEntity).FloatingTextGenerator.AddTextDisplay(new Common.UI.TextDisplay() { text = "-" + value + " AP" });
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
                    return "Action Points";
                case StatType.APGain:
                    return "AP Gain";
                case StatType.Armour:
                    return "Armour";
                case StatType.Health:
                    return "Health";
                case StatType.Inteligence:
                    return "Inteligence";
                case StatType.Movement:
                    return "Movement Points";
                case StatType.Speed:
                    return "Speed";
                case StatType.Strength:
                    return "Strength";
            }
            return "";
        }
        public static string StatTypeToTooltip(StatType type)
        {
            switch (type)
            {
                case StatType.AP:
                    return "(AP) Used to perform actions, everyone gains 2 per turn";
                case StatType.Armour:
                    return "Negates Physical Damage";
                case StatType.Health:
                    return "Dies when this reaches zero";
                case StatType.Inteligence:
                    return "LOL THIS ISNT BEING USED YET DELETE THIS";
                case StatType.Movement:
                    return "(MP) Can spend one ap fill momement, spend one movement to move one square";
                case StatType.Speed:
                    return "Determines who goes first, movement is the tie breaker";
                case StatType.Strength:
                    return "Base damage that is dealt with skills";
            }
            return "";
        }
    }

    public enum StatType { Movement, Health, Armour, Strength, Inteligence, Speed, AP, APGain }

}