using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class PassiveGeneric : Passive
    {
        public PassiveGeneric(string title, string description, List<SkillModifier> skillModifiers)
        {
            this.title = title;
            this.description = description;
            this.skillModifiers = skillModifiers;
        }


    }
}
