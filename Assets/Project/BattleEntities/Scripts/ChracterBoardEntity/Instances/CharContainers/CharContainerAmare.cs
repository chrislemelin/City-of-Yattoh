using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Entities.Instances
{

    public class CharContainerAmare : CharContainer
    {
        public override void Init(CharacterBoardEntity character)
        {
            character.AddPassive(new PassiveAmare());
            character.AddPassive(new TalentAmare());
            character.AddPassive(new TalentTriggerAmare());

            character.AddSkill(new SkillAmare1());
            character.AddSkill(new SkillAmare2());
        }
    }
}
