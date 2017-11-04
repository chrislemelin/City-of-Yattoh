using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatInstance
{
    [SerializeField]
    private int armour;

    [SerializeField]
    private int inteligence;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private int movement;

    [SerializeField]
    private int speed;

    [SerializeField]
    private int strength;

    private Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    public StatInstance()
    {
        stats = new Dictionary<StatType, Stat>();
    }

    public StatInstance(StatInstance obj)
    {
        stats.Clear();
        foreach (StatType type in obj.stats.Keys)
        {
            Stat stat = obj.stats[type];
            stats.Add(type, new Stat(stat.Name, stat.Value));
        }
    }

    public void UseDefaults()
    {
        stats.Clear();
        stats.Add(StatType.Armour, new Stat("Armour", armour));
        stats.Add(StatType.Inteligence, new Stat("Inteligence", inteligence));
        stats.Add(StatType.MaxHealth, new Stat("MaxHealth", maxHealth));
        stats.Add(StatType.Movement, new Stat("Movement", movement));
        stats.Add(StatType.Speed, new Stat("Speed", speed));
        stats.Add(StatType.Strength, new Stat("Strength", strength));
    }

    public int getValue(StatType type)
    {
        if(stats.ContainsKey(type))
        {
            return (int)stats[type].Value;
        }
        return -1;
    }

    public void ApplyMod(StatModifier mod)
    {
        if (stats.ContainsKey(mod.StatType))
        {   
            Stat newStat = new Stat(stats[mod.StatType].Name, mod.Apply(stats[mod.StatType].Value, mod.StatType));
            stats.Remove(mod.StatType);
            stats.Add(mod.StatType, newStat);
        }
        else
        {
            Console.WriteLine("doesnt have stat of type " + mod.StatType);
        }
    }

    public int GetStat(StatType type)
    {
        if (stats.ContainsKey(type))
        {
            return (int)stats[type].Value;
        }
        return -1;
    }

    public IEnumerable<Stat> GetStats()
    {
        return stats.Values;
    }


}
