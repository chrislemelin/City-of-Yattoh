using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.Battle.Entities.Instances
{

    public class CharContainerDadi : CharContainer
    {


        public CharContainerDadi()
        {
            talent = new TalentDadi();
            talentTrigger = new TalentTriggerDadi();
            passive = new PassiveDadi();
            skills.Add(new SkillDadi1());
            skills.Add(new SkillDadi2());
        }
    }
}
