using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Instances
{

    public class CharContainerBongani : CharContainer
    {
        public override void Init(CharacterBoardEntity character)
        {
            character.AddPassive(new PassiveBongani());
            character.AddPassive(new TalentBongani2());
            character.AddPassive(new TalentTriggerBongani2());
            character.AddSkill(new SkillBongani3());
            character.AddSkill(new SkillBongani2());
        }
    
    }
}
