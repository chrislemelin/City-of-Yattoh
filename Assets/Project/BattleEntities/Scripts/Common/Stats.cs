using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stats {

    public delegate void UpdateState(object sender);
    public event UpdateState updateStatHandler;

    private List<StatModifier> modifiers = new List<StatModifier>();

    [SerializeField]
    private StatInstance baseStats;
    public StatInstance BaseStats
    {
        get { return baseStats; }
    }

    public void Start()
    {
        baseStats.UseDefaults();
        movementPoints = baseStats.GetStat(StatType.Movement);
    }

    public StatInstance GetStatInstance()
    {
        StatInstance returnStats = new StatInstance(baseStats);
        foreach(StatModifier modifier in modifiers)
        {
            returnStats.ApplyMod(modifier);

        }
        return returnStats;

    }

    private int movementPoints;
    public int MovementPoints
    {
        get { return movementPoints; }
        private set {
            movementPoints = value;
            if(updateStatHandler != null)
            {
                updateStatHandler(this);
            }
        }
    }

    public void NewTurn()
    {
       MovementPoints = (int)baseStats.GetStat(StatType.Movement);
    }

    public void AddMovementPoint(int add)
    {
        MovementPoints += add;
    }

    public void ModifyMovementPoint(int sub)
    {
        MovementPoints -= sub;
    }

    private void UpdateGUI()
    {

    }

}

public enum StatType {Movement, MaxHealth, Armour, Strength, Inteligence, Speed}
