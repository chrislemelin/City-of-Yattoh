using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.Battle.Entities.Instances
{
    public class CharContainerTisha : CharContainer
    {
        public CharContainerTisha()
        {
            talent = new TalentTisha();
            talentTrigger = new TalentTriggerTisha();
            passive = new PassiveTisha();
            skills.Add(new SkillTisha1());
            skills.Add(new SkillTisha2());
        }
    }
}
