using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Instances;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Common.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Placeholdernamespace.CharacterSelection
{

    public class CharacterSkillView : MonoBehaviour
    {

        [SerializeField]
        private bool kaPreview = false;

        [SerializeField]
        private GameObject skillOptionButton;

        [SerializeField]
        private GameObject skillOptionContainer;

        List<Skill> skills = new List<Skill>();
        CharacterBoardEntity boardEntity;
        List<GameObject> buttonList = new List<GameObject>();
        List<GameObject> buttonClickList = new List<GameObject>();

        Dictionary<Skill, GameObject> skillToButton = new Dictionary<Skill, GameObject>();
        Dictionary<Passive, GameObject> passiveToButton = new Dictionary<Passive, GameObject>();
        object selected;

        public void SetBoardEntity(CharacterBoardEntity boardEntity)
        {
            skillToButton.Clear();
            buttonClickList.Clear();
            if (boardEntity != null)
            {
                Show();
                this.boardEntity = boardEntity;
                SetSkills(boardEntity.Skills);
                SetPassives(boardEntity.Passives);
                SkillButtonClick(boardEntity.Skills[0]);
            }
            else
            {
                Hide();
            }

        }

        public void SetKa(Ka ka)
        {
            foreach(Skill skill in ka.Skills)
            {
                SkillButtonClick(skill);
            }
            foreach (Passive passive in ka.Passives)
            {
                if(!(passive is Talent))
                    PassiveButtonClick(passive);
            }
        }

        private bool ActiveSkill(Skill skill)
        {
            if(kaPreview)
            {
                return true;
            }
            return false;
        }

        private bool ActivePassive(Passive passive)
        {
            if (kaPreview)
            {
                if(!(passive is Talent))
                {
                    return true;
                }
            }
            return false;
        }

        public void SetSkills(List<Skill> newSkills)
        {
            skills = newSkills;
            Show();

            ClearButtonList();
            foreach (Skill skill in skills)
            {
                BuildSkillButton(skill);
            }
        }

        public void SetPassives(List<Passive> newPassives)
        {
            foreach(Passive passive in newPassives)
            {
                BuildPassiveButton(passive);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void InitKa(Ka ka)
        {
            if(selected != null && ka != null)
            {
                if(selected is Skill)
                {
                    ka.AddSkill((Skill)selected);
                }
                if (selected is Passive)
                {
                    ka.AddPassive((Passive)selected);
                }
            }
        }

        private void SkillButtonClick(Skill skill)
        {
            if(kaPreview)
            {
                selected = skill;
                SetColors(skillToButton[skill]);
            }

        }

        private void PassiveButtonClick(Passive passive)
        {
            if(kaPreview)
            {
                selected = passive;
                SetColors(passiveToButton[passive]);
            }
        }

        private void SetColors(GameObject buttonClicked)
        {
            foreach(GameObject button in buttonClickList)
            {
                if(button == buttonClicked)
                {
                    button.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    button.GetComponent<Image>().color = Color.grey;
                }
            }
        }

        private GameObject BuildSkillButton(Skill skill)
        {
            GameObject skillButton = buildSkillButton(skill.GetTitle(), () => SkillButtonClick(skill), skill.GetDescription,
                skill.GetFlavorText, () => ActiveSkill(skill));
            skillToButton.Add(skill, skillButton);
            buttonList.Add(skillButton);
            buttonClickList.Add(skillButton);
            return skillButton;
        }

        private GameObject BuildPassiveButton(Passive passive)
        {
            Color? color = null;
            if(passive is Talent && kaPreview)
            {
                color = Color.green;
            }
            GameObject skillButton = buildSkillButton(passive.GetTitle(), () => PassiveButtonClick(passive), passive.GetDescription,
              () => null, () => ActivePassive(passive), color);
            passiveToButton.Add(passive, skillButton);
  
             buttonList.Add(skillButton);
            if (!(passive is Talent))
                buttonClickList.Add(skillButton);

            return skillButton;
        }

        private string returnNull()
        {
            return null;
        }

        private GameObject buildSkillButton(string title, Action onClick, Func<String> getDescription,
            Func<string> getFlavorText, Func<bool> active, Color? color = null)
        {
            bool tempActive = active();
            skillOptionButton.GetComponent<Button>().interactable = tempActive;
            GameObject skillButton = Instantiate(skillOptionButton);
            skillButton.GetComponent<TooltipSpawner>().Init(() => { return null; }, getDescription, getFlavorText);
            skillButton.GetComponent<Button>().interactable = tempActive;
            skillButton.GetComponentInChildren<TextMeshProUGUI>().text = title;
            skillButton.transform.SetParent(skillOptionContainer.transform, false);
            skillButton.GetComponent<Button>().onClick.AddListener(() => onClick());
            if (color != null)
            {
                skillButton.GetComponent<Image>().color = (Color)color;
            }
            skillButton.SetActive(true);
            return skillButton;
        }



        private void ClearButtonList()
        {
            foreach (GameObject g in buttonList)
            {
                Destroy(g);
            }
            buttonList.Clear();
            skillToButton.Clear();
            passiveToButton.Clear();
        }

    }
}

