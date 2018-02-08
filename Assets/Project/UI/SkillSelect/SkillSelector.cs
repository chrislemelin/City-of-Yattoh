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
using Placeholdernamespace.Battle.Managers;

namespace Placeholdernamespace.Battle.Interaction
{
    public class SkillSelector : MonoBehaviour {

        private CharacterBoardEntity boardEntity;

        [SerializeField]
        private GameObject skillOptionButton;

        [SerializeField]
        private GameObject skillOptionContainer;

        private List<GameObject> skillButtons = new List<GameObject>();
        private Dictionary<Button, Func<bool>> buttonToActive = new Dictionary<Button, Func<bool>>();

        private List<Skill> skills;
        private Skill selectedSkill;
        private Action skillSelected;
        private Profile profile;
        private GameObject cancelButton;
        private TurnManager turnManager;

        public Skill SelectedSkill
        {
            get { return selectedSkill; }
        }

        private TileSelectionManager tileSelectionManager;
        private Action selectionCancel;
        private Func<BoardEntity> getHoverEntity;

        public void Init(TileSelectionManager tileSelectionManager, Action selectionCancel, Func<BoardEntity> getHoverEntity, Profile profile, TurnManager turnManager)
        {
            this.tileSelectionManager = tileSelectionManager;
            this.selectionCancel = selectionCancel;
            this.profile = profile;
            this.getHoverEntity = getHoverEntity;
            this.turnManager = turnManager;
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
            buildEndTurnButton();

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

            if (skill.SelfCasting())
            {
                selectedSkill.Action(new List<Tile>(), (bool ok) => ExecuteSkillCallback());
            }
            else
            {
                ClearButtonList();
                buildCancelSkillButton();
                List<TileSelectOption> tileSelectOptions = skill.TileOptionSet();
                foreach (TileSelectOption option in tileSelectOptions)
                {
                    if (option.DisplayStats != null)
                    {
                        option.OnHoverAction = () =>
                        {
                            if (getHoverEntity() == null)
                            { profile.UpdateProfile(option.DisplayStats.BoardEntity, skillReport: option.skillReport); }
                        };
                    }
                }
                tileSelectionManager.SelectTile(boardEntity, tileSelectOptions, ExecuteSkill);

            }


            //tileSelectionManager.SelectTile(boardEntity, skill.TileSet(), ExecuteSkill, Color.blue, Color.cyan);
        }

        private GameObject buildSkillButton(Skill skill)
        {
            GameObject skillButton = buildSkillButton(skill.GetTitle(), () => { SetSelectedSkill(skill); }, skill.GetDescription, 
                skill.GetFlavorText, skill.IsActive);
            return skillButton;
        }

        private void buildCancelSkillButton()
        {
            cancelButton = buildSkillButton("Cancel", () => { tileSelectionManager.CancelSelection(); ExecuteSkill(null); }, returnNull,
                returnNull, defaultActive, Color.white);
        }

        private void buildEndTurnButton()
        {
            cancelButton = buildSkillButton("End Turn", boardEntity.EndMyTurn, returnNull,
              returnNull, defaultActive, Color.white);
        }

        private bool defaultActive()
        {
            return !PathOnClick.pause;
        }

        private string returnNull()
        {
            return null;
        }

        private GameObject buildSkillButton(string title, Action onClick, Func<String> getDescription ,
            Func<string> getFlavorText, Func<bool> active, Color? color = null)
        {
            GameObject skillButton = Instantiate(skillOptionButton);
            skillButton.GetComponent<TooltipSpawner>().Init(() => { return null; }, getDescription, getFlavorText);
            skillButton.GetComponent<Button>().interactable = active();            
            skillButton.GetComponentInChildren<TextMeshProUGUI>().text = title;
            skillButton.transform.SetParent(skillOptionContainer.transform, false);
            skillButton.GetComponent<Button>().onClick.AddListener(() => onClick());   
            buttonToActive.Add(skillButton.GetComponent<Button>(), active);
            skillButtons.Add(skillButton);
            if (color != null)
            {                
                skillButton.GetComponent<Image>().color = (Color)color;
            }
            skillButton.SetActive(true);
            return skillButton;
        }

        private void OnGUI()
        {
            foreach(KeyValuePair<Button, Func<bool>> value in buttonToActive)
            {
                value.Key.interactable = value.Value();
            }
        }

        private void ExecuteSkill(TileSelectOption tile)
        {
            profile.UpdateProfile(null);
            if(tile != null)
            {
                if (cancelButton != null)
                    cancelButton.GetComponent<Button>().interactable = false;
                selectedSkill.Action((List<Tile>)tile.ReturnObject, (bool ok) => ExecuteSkillCallback());
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
            buttonToActive.Clear();
            skillButtons.Clear();
            cancelButton = null;
        }

    }


}
