using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Instances
{

    public class CharContainerLesidi : CharContainer
    {

        // Use this for initialization
        public override void Init(CharacterBoardEntity character)
        {
            character.setRange(4);

            character.AddPassive(new PassiveLesidi());
            character.AddPassive(new TalentTriggerLesidi());
            character.AddPassive(new TalentLesidi());
            character.AddSkill(new SkillLesidi1());
            character.AddSkill(new SkillLesidi2());
        }
    }
}
