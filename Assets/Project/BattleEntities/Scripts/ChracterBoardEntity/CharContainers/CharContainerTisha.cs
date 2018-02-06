using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.Battle.Entities.Instances
{
    public class CharContainerTisha : CharContainer
    {
        public override void Init(CharacterBoardEntity character)
        {
            character.AddPassive(new PassiveTisha());
            character.AddPassive(new TalentTisha());
            character.AddPassive(new TalentTriggerTisha());

            character.AddSkill(new SkillTisha1());
            character.AddSkill(new SkillTisha2());
        }
    }
}
