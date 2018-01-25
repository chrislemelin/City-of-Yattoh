using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Instances
{
    public class SkillsContainer : MonoBehaviour
    {
        protected List<Passive> passives;
        protected List<Skill> skills;

        protected void AddPassive(Passive p)
        {
            passives.Add(p);
        }
        protected void AddSkill(Skill s)
        {
            skills.Add(s);
        }

        public void AddToBoardEntity(CharacterBoardEntity character)
        {
            foreach(Passive p in passives)
            {
                character.AddPassive(p);
            }
            foreach(Skill s in skills)
            {
                character.AddSkill(s);
            }
        }

    }
}
