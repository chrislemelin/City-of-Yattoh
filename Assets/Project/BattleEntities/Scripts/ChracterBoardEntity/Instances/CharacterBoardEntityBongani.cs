using Placeholdernamespace.Battle.Entities.Passives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities
{
    public class CharacterBoardEntityBongani : CharacterBoardEntity
    {

        public CharacterBoardEntityBongani():base()
        {
            AddPassive(new PassiveAreaOfInfluenceSkill());
            AddPassive(new TalentBongani());
            AddPassive(new TalentTriggerBongani());
            //AddPassive()
        }
        
    }
}
