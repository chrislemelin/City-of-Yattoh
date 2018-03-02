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
        private Sprite profilePic;
        public Sprite ProfilePic
        {
            get { return profilePic; }
        }   

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

        private CharacterType characterType;
        public CharacterType CharacterType
        {
            get { return characterType; }
        }

        Dictionary<CharacterType, Color> typeToColor = new Dictionary<CharacterType, Color>() {
            { CharacterType.PlayerAmare, Color.red},
            { CharacterType.PlayerBongani, Color.red},
            { CharacterType.PlayerDadi, Color.red},
            { CharacterType.PlayerJaz, Color.red},
            { CharacterType.PlayerLesidi, Color.red},
            { CharacterType.PlayerTisha, Color.red},
        };

        private Color kaColor = Color.grey;
        public Color KaColor
        {
            get { return kaColor; }
        }


        public Ka(CharContainer character)
        {
            characterType = character.Type;
            if(typeToColor.ContainsKey(character.Type))
            {
                kaColor = typeToColor[character.Type];
            }
            talent = character.Talent;
            AddPassive(((Passive)talent));
            profilePic = character.GetComponent<CharacterBoardEntity>().ProfileImageCircle;
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
            foreach(Passive passive in passives)
            {
                character.InitPassive(passive);
            }
            foreach(Skill skill in Skills)
            {
                character.InitSkill(skill);
            }
        }


    }
}
