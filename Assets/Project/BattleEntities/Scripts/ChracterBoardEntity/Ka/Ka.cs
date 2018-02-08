using Placeholdernamespace.Battle.Entities.Instances;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Kas
{
    public class Ka
    {
        private Talent talent;
        public Talent Talent
        {
            get { return talent; }
        }

        private CharacterBoardEntity character;
        private List<Skill> skills = new List<Skill>();
        public List<Skill> Skills
        {
            get { return new List<Skill>(skills); }
        }

        private List<Passive> passives = new List<Passive>();
        public List<Passive> Passives
        {
            get { return new List<Passive>(passives); }
        }

        public Ka(CharContainer character)
        {
            talent = character.Talent;
        }

        public void AddPassive(Passive passive)
        {
            passives.Add(passive);
        }

        public void AddSkill(Skill skill)
        {
            skills.Add(skill);
        }

        public void Init(CharacterBoardEntity character)
        {
            this.character = character;
            foreach(Passive p in passives)
            {
                character.AddPassive(p);
            }
            foreach(Skill skill in skills)
            {
                character.AddSkill(skill);
            }

        }


    }
}
