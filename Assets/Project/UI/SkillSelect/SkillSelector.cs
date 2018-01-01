using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Placeholdernamespace.Battle.Interaction
{
    public class SkillSelector : MonoBehaviour {

        private CharacterBoardEntity boardEntity;

        [SerializeField]
        private GameObject skillOptionButton;

        [SerializeField]
        private GameObject skillOptionContainer;

        private List<GameObject> skillButtons = new List<GameObject>();
        private List<Skill> skills;
        private Skill selectedSkill;
        private Action skillSelected;

        public Skill SelectedSkill
        {
            get { return selectedSkill; }
        }

        private TileSelectionManager tileSelectionManager;
        private Action selectionCancel;    

        public void Init(TileSelectionManager tileSelectionManager, Action selectionCancel, Action skillSelected)
        {
            this.tileSelectionManager = tileSelectionManager;
            this.selectionCancel = selectionCancel;
            this.skillSelected = skillSelected;
        }

        public void SetBoardEntity(CharacterBoardEntity boardEntity)
        {
            this.boardEntity = boardEntity;
        }

        public void SetSkills(List<Skill> newSkills)
        {
            skills = newSkills;
            Show();

            ClearButtonList();
            foreach (Skill skill in skills)
            {
                buildSkillButton(skill);
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

        public void SetSelectedSkill(Skill skill)
        {
            selectedSkill = skill;           
            tileSelectionManager.CancelSelection();
            ClearButtonList();
            buildCancelSkillButton();
            
            tileSelectionManager.SelectTile(boardEntity, skill.TileSet(), ExecuteSkill, Color.blue, Color.cyan);
        }

        private void buildSkillButton(Skill skill)
        {
            bool interactable = skill.IsActive();
            GameObject skillButton = buildSkillButton(skill.Title, () => { SetSelectedSkill(skill); }, interactable);
        }

        private void buildCancelSkillButton()
        {
            buildSkillButton("Cancel", () => { tileSelectionManager.CancelSelection(); ExecuteSkill(null); });
        }

        private GameObject buildSkillButton(string title, Action onClick, bool interactable = true)
        {
            GameObject skillButton = Instantiate(skillOptionButton);
            skillButton.GetComponent<Button>().interactable = interactable;
            skillButton.GetComponentInChildren<Text>().text = title;
            skillButton.transform.SetParent(skillOptionContainer.transform);
            skillButton.GetComponent<Button>().onClick.AddListener(() => onClick());
            skillButtons.Add(skillButton);
            skillButton.SetActive(true);
            return skillButton;
        }

        private void ExecuteSkill(TileSelectOption tile)
        {
            if(tile != null)
            {
                selectedSkill.Action(tile.Selection);
                selectedSkill = null;
                skillSelected();
            }
            else
            {
                ClearButtonList();
                selectionCancel();
                selectedSkill = null;
            }
        }

        private void ClearButtonList()
        {
            foreach (GameObject g in skillButtons)
            {
                Destroy(g);
            }
            skillButtons.Clear();
        }

    }


}
