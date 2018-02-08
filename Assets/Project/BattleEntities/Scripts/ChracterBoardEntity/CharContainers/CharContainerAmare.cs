using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Instances
{

    public class CharContainerAmare : CharContainer
    {
        public CharContainerAmare()
        {
            talent = new TalentAmare();
            talentTrigger = new TalentTriggerAmare();
            passive = new PassiveAmare();
            skills.Add(new SkillAmare1());
            skills.Add(new SkillAmare2());
        }
    }
}
