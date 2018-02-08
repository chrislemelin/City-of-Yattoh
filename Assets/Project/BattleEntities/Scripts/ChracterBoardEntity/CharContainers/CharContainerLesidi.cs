using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Instances
{

    public class CharContainerLesidi : CharContainer
    {
        public CharContainerLesidi()
        {
            range = 3;
            talent = new TalentLesidi();
            talentTrigger = new TalentTriggerLesidi();
            passive = new PassiveLesidi();
            skills.Add(new SkillLesidi1());
            skills.Add(new SkillLesidi2());
        }
    
    }
}
