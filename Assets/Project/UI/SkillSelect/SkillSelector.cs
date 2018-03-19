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

namespace Placeholdernamespace.Battle.UI
{
    
    enum State { start, move, skill }

    public class SkillSelector : BaseSelector {
        

        private CharacterBoardEntity boardEntity;

        private List<Skill> skills;
        private Skill selectedSkill;
        private Profile profile;
        private Action doneAction;

        public Skill SelectedSkill
        {
            get { return selectedSkill; }
        }

        private TileSelectionManager tileSelectionManager;
        private Action selectionCancel;

        public void Init(TileSelectionManager tileSelectionManager, Transform targetTransform, Profile profile, GameObject parent)
        {
            this.tileSelectionManager = tileSelectionManager;
            this.profile = profile;
            base.Init(targetTransform, parent);
        }

        public void SetDoneAction(Action doneAction)
        {
            this.doneAction = () => { ClearButtons(); tileSelectionManager.CancelSelection(); doneAction(); };

        }

        public void SetSkills(List<Skill> newSkills)
        {
            skills = newSkills;
            ClearButtons();
            foreach (Skill skill in skills)
            {
                buildSkillButton(skill);
            }
            buildCancelSkillButton(doneAction);

        }

        public void SetSelectedSkill(Skill skill)
        {
            selectedSkill = skill;           
            tileSelectionManager.CancelSelection();

            if (skill.SelfCasting())
            {
                selectedSkill.Action(new List<Tile>(), ExecuteSkillCallback);
            }
            else
            {
                ClearButtons();
                buildCancelSkillButton(() => { tileSelectionManager.CancelSelection(); ExecuteSkill(null); } );
                List<TileSelectOption> tileSelectOptions = skill.TileOptionSet();
                foreach (TileSelectOption option in tileSelectOptions)
                {
                    if (option.DisplayStats != null)
                    {
                        option.OnHoverAction = () =>
                        {
                            profile.UpdateProfile(option.DisplayStats.BoardEntity, skillReport: option.skillReport); 
                        };
                    }
                }
                tileSelectionManager.SelectTile(boardEntity, tileSelectOptions, ExecuteSkill);

            }
           // buildCancelSkillButton(doneAction);


            //tileSelectionManager.SelectTile(boardEntity, skill.TileSet(), ExecuteSkill, Color.blue, Color.cyan);
        }

        private GameObject buildSkillButton(Skill skill)
        {
            GameObject skillButton = buildButton(skill.GetTitle(), () => { SetSelectedSkill(skill); }, skill.GetDescription, 
                skill.GetFlavorText, skill.IsActive);
            return skillButton;
        } 

        private void ExecuteSkill(TileSelectOption tile)
        {
            
            //profile.UpdateProfile(null);
            if(tile != null)
            {
                if (cancelButton != null)
                    cancelButton.GetComponent<Button>().interactable = false;
                SetInteractableCancelButton(false);
                selectedSkill.Action((List<Tile>)tile.ReturnObject,  ExecuteSkillCallback);

            }
            else
            {
                ExecuteSkillCallback();
            }
        }
        private void ExecuteSkillCallback()
        {           
            selectedSkill = null;
            SetSkills(skills);
        }

        

    
      
    }
    

}
