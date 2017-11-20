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

        private BoardEntity boardEntity;

        [SerializeField]
        private GameObject skillOptionButton;

        [SerializeField]
        private GameObject skillOptionContainer;

        private List<GameObject> skillButtons = new List<GameObject>();
        private List<Skill> skills;
        private Skill selectedSkill;
        public Skill SelectedSkill
        {
            get { return selectedSkill; }
        }

        private TileSelectionManager tileSelectionManager;
        private Action selectionCancel;    

        public void Init(TileSelectionManager tileSelectionManager, Action selectionCancel)
        {
            this.tileSelectionManager = tileSelectionManager;
            this.selectionCancel = selectionCancel;
        }

        public void SetBoardEntity(BoardEntity boardEntity)
        {
            this.boardEntity = boardEntity;
        }

        public void SetSkills(List<Skill> newSkills)
        {
            skills = newSkills;
            gameObject.SetActive(true);

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
            GameObject skillButton = Instantiate(skillOptionButton);
            skillButton.GetComponentInChildren<Text>().text = skill.Title;
            skillButton.transform.SetParent(skillOptionContainer.transform);
            skillButton.GetComponent<Button>().onClick.AddListener(() => { SetSelectedSkill(skill); Show();  });
            skillButtons.Add(skillButton);
        }

        private void buildCancelSkillButton()
        {
            GameObject skillButton = Instantiate(skillOptionButton);
            skillButton.GetComponentInChildren<Text>().text = "Cancel";
            skillButton.transform.SetParent(skillOptionContainer.transform);
            skillButton.GetComponent<Button>().onClick.AddListener(() => { tileSelectionManager.CancelSelection(); ExecuteSkill(null); } );
            skillButtons.Add(skillButton);
        }

        private void ExecuteSkill(TileSelectOption tile)
        {
            if(tile != null)
            {
                selectedSkill.Action(tile.Selection);
                selectedSkill = null;
                Hide();
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
