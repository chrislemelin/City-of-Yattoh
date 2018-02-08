using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Instances
{
    public class CharContainerJaz : CharContainer {

        public CharContainerJaz()
        {
            range = 4;
            talent = new TalentJaz();
            talentTrigger = new TalentTriggerJaz();
            passive = new PassivePiercingJaz();
            skills.Add(new SkillJaz1());
            skills.Add(new SkillJaz2());
        }

    }
}