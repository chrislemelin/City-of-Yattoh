using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.UI;
using Placeholdernamespace.Common.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        private Profile profile;
        private GameObject cancelButton;

        public Skill SelectedSkill
        {
            get { return selectedSkill; }
        }

        private TileSelectionManager tileSelectionManager;
        private Action selectionCancel;    

        public void Init(TileSelectionManager tileSelectionManager, Action selectionCancel, Profile profile)
        {
            this.tileSelectionManager = tileSelectionManager;
            this.selectionCancel = selectionCancel;
            this.profile = profile;
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
            List<TileSelectOption> tileSelectOptions = skill.TileOptionSet();
            foreach(TileSelectOption option in tileSelectOptions)
            {
                option.OnHoverAction = () => profile.UpdateProfile(option.DisplayStats.BoardEntity, option.DisplayStats);
            }

            tileSelectionManager.SelectTile(boardEntity, tileSelectOptions, ExecuteSkill);
            
            //tileSelectionManager.SelectTile(boardEntity, skill.TileSet(), ExecuteSkill, Color.blue, Color.cyan);
        }

        private GameObject buildSkillButton(Skill skill)
        {
            bool interactable = skill.IsActive();
            GameObject skillButton = buildSkillButton(skill.GetTitle(), () => { SetSelectedSkill(skill); }, skill.GetDescription, interactable);
            return skillButton;
        }

        private void buildCancelSkillButton()
        {
            cancelButton = buildSkillButton("Cancel", () => { tileSelectionManager.CancelSelection(); ExecuteSkill(null); },() => { return null; } );
        }

        private GameObject buildSkillButton(string title, Action onClick, Func<String> getDescription, bool interactable = true)
        {
            GameObject skillButton = Instantiate(skillOptionButton);
            skillButton.GetComponent<TooltipSpawner>().Init(() => { return null; }, getDescription);
            skillButton.GetComponent<Button>().interactable = interactable;
            skillButton.GetComponentInChildren<TextMeshProUGUI>().text = title;
            skillButton.transform.SetParent(skillOptionContainer.transform);
            skillButton.GetComponent<Button>().onClick.AddListener(() => onClick());
            skillButtons.Add(skillButton);
            skillButton.SetActive(true);
            return skillButton;
        }

        private void ExecuteSkill(TileSelectOption tile)
        {
            profile.UpdateProfile(null);
            if(tile != null)
            {
                if (cancelButton != null)
                    cancelButton.GetComponent<Button>().interactable = false;
                selectedSkill.Action(tile.Selection, ExecuteSkillCallback);
            }
            else
            {
                ExecuteSkillCallback();
            }
        }
        private void ExecuteSkillCallback()
        {
            ClearButtonList();
            selectionCancel();
            selectedSkill = null;
        }

        private void ClearButtonList()
        {
            foreach (GameObject g in skillButtons)
            {
                Destroy(g);
            }
            skillButtons.Clear();
            cancelButton = null;
        }

    }


}
