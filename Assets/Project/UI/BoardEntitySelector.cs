﻿using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Interaction
{
    public class BoardEntitySelector :MonoBehaviour{

        [SerializeField]
        private SkillSelector skillSelector;
        public SkillSelector SkillSelector
        {
            get { return SkillSelector; }
        }

        [SerializeField]
        private TileSelectionManager tileSelectionManager;
        public TileSelectionManager TileSelectionManager
        {
            get { return tileSelectionManager; }
        }

        [SerializeField]
        private Profile profile;

        private BoardEntity selectedBoardEntity;
        public BoardEntity SelectedBoardEntity
        {
            get { return selectedBoardEntity; }
        }
        
        public void Init()
        {
            tileSelectionManager.Init(profile);
            skillSelector.Init(tileSelectionManager, buildMoveOptions, ()=> setSelectedBoardEntity(null));
        }

        public void setSelectedBoardEntity(BoardEntity boardEntity)
        {             
            profile.UpdateProfile(boardEntity);
            selectedBoardEntity = boardEntity;
            buildMoveOptions();
            if(boardEntity == null)
            {
                skillSelector.Hide();
            }
        }

        private void buildMoveOptions()
        {
            if (selectedBoardEntity is CharacterBoardEntity)
            {
                if (TurnManager.CurrentBoardEntity == selectedBoardEntity)
                {
                    tileSelectionManager.SelectTile(selectedBoardEntity, selectedBoardEntity.MoveSet(), sendMoveToBoardEntity, null, null);               
                    skillSelector.SetBoardEntity((CharacterBoardEntity)selectedBoardEntity);
                    skillSelector.SetSkills(selectedBoardEntity.Skills);
                }
                else
                {
                    tileSelectionManager.SelectTile(selectedBoardEntity, new List<Move>() , sendMoveToBoardEntity, null, null);
                }
            }
        }

        private void sendMoveToBoardEntity(TileSelectOption tileOption)
        {
            if (selectedBoardEntity is CharacterBoardEntity)
            {
                if(tileOption != null)
                {
                    ((CharacterBoardEntity)selectedBoardEntity).ExecuteMove((Move)tileOption.ReturnObject);
                }
                else
                {
                    ((CharacterBoardEntity)selectedBoardEntity).ExecuteMove(null);
              
                }
                if(skillSelector.SelectedSkill == null)
                {
                    setSelectedBoardEntity(null);
                    skillSelector.Hide();
                }
            }
        }
    }
}