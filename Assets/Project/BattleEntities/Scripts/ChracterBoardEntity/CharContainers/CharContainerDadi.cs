using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.Battle.Entities.Instances
{

    public class CharContainerDadi : CharContainer
    {

        public override void Init(CharacterBoardEntity character)
        {
            
            character.AddPassive(new TalentTriggerDadi());
            character.AddPassive(new PassiveDadi());
            character.AddPassive(new TalentDadi());
            character.AddSkill(new SkillDadi1());
            character.AddSkill(new SkillDadi2());

        }
    }
}
