using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{

    public class TalentTriggerBongani : TalentTrigger
    {

        public TalentTriggerBongani()
        {
            description = "TALENT Apply a -1 strength debuff on all adjacent allies";
        }

        public override void AboutToExecuteAction(Skill skill, List<Tile> tiles)
        {
            if(skill is BasicAttack)
            {
                CharacterBoardEntity character = tileManager.GetFirstCharacter(tiles);
                if(character.HasPassiveType(PassiveType.Debuff))
                {
                    Trigger();
                }
            }
        }

    }
}
