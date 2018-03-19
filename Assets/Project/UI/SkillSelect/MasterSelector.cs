using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.UI
{
    public class MasterSelector : BaseSelector
    {

        [SerializeField]
        MoveSelector moveSelector;

        [SerializeField]
        SkillSelector skillSelector;

        [SerializeField]
        Transform masterTargetTransform;

        [SerializeField]
        TileSelectionManager tileSelectionManager;

        [SerializeField]
        Profile profile;

        [SerializeField]
        GameObject masterParent;

        private CharacterBoardEntity selectedBoardEntity;

        

        public void Awake()
        {
            moveSelector.Init(tileSelectionManager, masterTargetTransform, masterParent);
            skillSelector.Init(tileSelectionManager, masterTargetTransform, profile, masterParent);
            base.Init(masterTargetTransform, masterParent);
            Hide();
        }

        public void SetPreviewBoardEntity(CharacterBoardEntity characterBoardEntity)
        {
            moveSelector.BuildPreviewMoveOptions(characterBoardEntity);
        }

        public void SetBoardEntity(CharacterBoardEntity selectedBoardEntity)
        {
            Show();
            this.selectedBoardEntity = selectedBoardEntity;
            moveSelector.SetDoneAction(() => { ClearButtons(); BuildOptions(selectedBoardEntity); });
            skillSelector.SetDoneAction(() => { ClearButtons(); BuildOptions(selectedBoardEntity); });
            ClearButtons();
            BuildOptions(selectedBoardEntity);
        
            

        }

        public void BuildOptions(CharacterBoardEntity selectedBoardEntity)
        {
            buildButton("Move", () => { ClearButtons(); moveSelector.BuildMoveOptions(selectedBoardEntity); },
             () => { return "See availible moves"; }, ReturnNull, () => { return true; });

            buildButton("Skills", () => { ClearButtons(); skillSelector.SetSkills(selectedBoardEntity.Skills); },
             () => { return "See availible skills"; }, ReturnNull, () => { return true; });

            if(selectedBoardEntity != null)
                buildCancelSkillButton(selectedBoardEntity);

        }

   
    }
}
