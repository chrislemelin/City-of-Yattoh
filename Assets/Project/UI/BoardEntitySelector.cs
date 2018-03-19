using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.UI
{
    public class BoardEntitySelector :MonoBehaviour{


        [SerializeField]
        private MasterSelector masterSelector;
        public MasterSelector MasterSelector
        {
            get { return masterSelector; }
        }

        [SerializeField]
        private TileSelectionManager tileSelectionManager;
        public TileSelectionManager TileSelectionManager
        {
            get { return tileSelectionManager; }
        }

        [SerializeField]
        private Profile profile;

        [SerializeField]
        private Profile previewProfile;

        [SerializeField]
        GameObject apDisplay;

        [SerializeField]
        private TurnManager turnManager;

        private BoardEntity selectedBoardEntity;
        public BoardEntity SelectedBoardEntity
        {
            get { return selectedBoardEntity; }
        }

        // profile that the user is viewing
        private BoardEntity hoverBoardEntity;

        public void Init()
        {
            tileSelectionManager.Init(profile);
            apDisplay.SetActive(false);
        }

        public void SetPreviewBoardEntity(CharacterBoardEntity boardEntity)
        {
            masterSelector.SetPreviewBoardEntity(boardEntity);
            previewProfile.UpdateProfile(boardEntity);
        }

        public void SetSelectedBoardEntity(BoardEntity boardEntity)
        {
            if(boardEntity == TurnManager.CurrentBoardEntity)
            {
                profile.UpdateProfile(boardEntity);
                SetPreviewBoardEntity(null);
                if (boardEntity == null || boardEntity.Team != Team.Enemy)
                {
                    tileSelectionManager.CancelSelection();
                    apDisplay.SetActive(true);
                    if (selectedBoardEntity != boardEntity)
                    {
                        masterSelector.SetBoardEntity((CharacterBoardEntity)boardEntity);

                    }
                    masterSelector.Show();
                    selectedBoardEntity = boardEntity;
                }
                else
                {
                    masterSelector.Hide();
                }

            }
            else
            {
                SetPreviewBoardEntity((CharacterBoardEntity)boardEntity);
            }
        }  

        public void HideSelector()
        {
            masterSelector.Hide();
            apDisplay.SetActive(false);
        }
     
    }
}
