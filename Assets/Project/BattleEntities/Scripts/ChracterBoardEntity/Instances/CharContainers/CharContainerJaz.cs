using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Instances
{
    public class CharContainerJaz : CharContainer {

        public override void Init(CharacterBoardEntity character)
        {
            character.setRange(4);

            character.AddPassive(new PassivePiercingJaz());
            character.AddPassive(new TalentJaz());
            character.AddPassive(new TalentTriggerJaz());

            character.AddSkill(new SkillJaz1());
            character.AddSkill(new SkillJaz2());
        }



    }
}