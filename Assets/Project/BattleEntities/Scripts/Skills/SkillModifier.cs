using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillModifier
    {
        private SkillModifierType skillModifierType;
        public SkillModifierType Type
        {
            get { return skillModifierType; }
        }
        private SkillModifierApplication skillModifierApplication;
        public SkillModifierApplication Application
        {
            get { return skillModifierApplication; }
        }
        private float value;
        public float Value
        {
            get { return value; }
        }

        public SkillModifier(SkillModifierType type, SkillModifierApplication application, float value)
        {
            skillModifierType = type;
            skillModifierApplication = application;
            this.value = value;
        }

        public float? Apply(float? value, SkillModifierType type)
        {
            // just double checking this modifier is being used on the right skill type
            if (type == this.Type)
            {
                switch (Application)
                {
                    case SkillModifierApplication.Add:
                        return value + this.value;
                    case SkillModifierApplication.AddNoMult:
                        return value + this.value;
                    case SkillModifierApplication.Mult:
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
    public enum SkillModifierType
    {
        Power, Range, CoolDown, APCost, Duration
    }

    public enum SkillModifierApplication
    {
        Add, Mult, AddNoMult
    }
}