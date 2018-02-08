using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Instances
{

    public class CharContainerBongani : CharContainer
    {

        public CharContainerBongani()
        {
            talent = new TalentBongani2();
            talentTrigger = new TalentTriggerBongani();
            passive = new PassiveBongani();
            skills.Add(new SkillBongani1());
            skills.Add(new SkillBongani2());
        }
    
    }
}
