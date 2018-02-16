using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Instances
{
    public class CharContainer : MonoBehaviour
    {
        protected int? range = Skill.RANGE_ADJACENT;
        public int? Range
        {
            get { return range; }
        }

        protected Talent talent;
        public Talent Talent
        {
            get { return talent; }
        }

        protected TalentTrigger talentTrigger;
        public TalentTrigger TalentTrigger
        {
            get { return talentTrigger; }
        }

        protected Passive passive;
        public Passive Passive
        {
            get { return passive; }
        }

        protected List<Skill> skills = new List<Skill>();
        public List<Skill> Skills
        {
            get { return skills; }
        }

        [SerializeField]
        protected CharacterType type;
        public CharacterType Type
        {
            get{return type;}
        }

        [SerializeField]
        private Sprite profileImage;

        public virtual void Init(CharacterBoardEntity character)
        {
            if (character.Passives.Count + character.Skills.Count <= 1)
            {

                if (passive != null)
                    character.AddPassive(passive);

                if (talentTrigger != null)
                    character.AddPassive(talentTrigger);

                if (talent != null)
                    character.AddPassive(talent);


                foreach (Skill skill in skills)
                {
                    character.AddSkill(skill);
                }

                character.SetCharacterType(type);
                character.setRange(range);

            }
        } 

    }
}
