using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier {

    private StatType statType;
    public StatType StatType
    {
        get { return statType; }
    }

    private StatModifierType modType;
    private float value;

    public StatModifier(StatType statType, StatModifierType modType, float value)
    {
        this.statType = statType;
        this.modType = modType;
        this.value = value;
    }
    
    public float Apply(float value, StatType statType)
    {
        if (statType == this.statType)
        {
            switch (modType)
            {
                case StatModifierType.Add:
                    return value + this.value;
                case StatModifierType.Mult:
                    return value * this.value;
                default:
                    return 0;
            }
        }
        else
        {
            return value;
        }
    }   

}

public enum StatModifierType {Add, Mult}
