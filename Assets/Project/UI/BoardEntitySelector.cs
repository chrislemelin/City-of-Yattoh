﻿using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
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
        Color SelectColor;

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

        [SerializeField]
        private List<Color> ApCostColors;

        [SerializeField]
        private Color hoverColor;

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
            skillSelector.Init(tileSelectionManager, () => { setSelectedBoardEntity(selectedBoardEntity); buildMoveOptions(); } ,
                getHoverEntity, profile, turnManager);
        }

        public void setSelectedBoardEntity(BoardEntity boardEntity)
        {
            
            if (boardEntity == TurnManager.CurrentBoardEntity && (boardEntity == null || boardEntity.Team != Team.Enemy ))
            {
                setHoverEntity(null);
                selectedBoardEntity = null;
                tileSelectionManager.CancelSelection();

                selectedBoardEntity = boardEntity;
                profile.UpdateProfile(boardEntity);

                buildMoveOptions();
                if (boardEntity == null)
                {
                    skillSelector.Hide();
                }
            }
            /*
            else
            {
                if (boardEntity == hoverBoardEntity)
                {
                    setHoverEntity(null);
                }
                else
                {
                    //setHoverEntity(boardEntity);
                }
            }
            */
        }

        private void setHoverEntity(BoardEntity boardEntity)
        {
            if (hoverBoardEntity != null)
            {
                hoverBoardEntity.GetTile().PathOnClick.ColorEffectManager.TurnOff(this);
            }
            hoverBoardEntity = boardEntity;
            if (hoverBoardEntity != null)
            {
                //profile.UpdateProfile(boardEntity);
                //hoverBoardEntity.GetTile().PathOnClick.ColorEffectManager.TurnOn(this, Color.blue);
            }

        }

        private BoardEntity getHoverEntity()
        {
            return hoverBoardEntity;
        }

        public void Hover(BoardEntity boardEntity)
        {
            if(getHoverEntity() == null)
            {
                profile.UpdateProfile(boardEntity);
            }
        }

        public void ExitHover()
        {
            //profile.UpdateProfile(selectedBoardEntity);
        }

        private void buildMoveOptions()
        {
            if (selectedBoardEntity is CharacterBoardEntity)
            {
                if (TurnManager.CurrentBoardEntity == selectedBoardEntity)
                {
                    List<Move> moveSet = selectedBoardEntity.MoveSet();
                    List<TileSelectOption> options = new List<TileSelectOption>();
                    foreach (Move m in moveSet)
                    {
                        Stats displaystats = selectedBoardEntity.Stats.GetCopy();
                        displaystats.SubtractAPPoints(m.apCost);
                        displaystats.SetMutableStat(StatType.Movement, m.movementPointsAfterMove);
                        Color col = ApCostColors[0];
                        if (m.apCost < ApCostColors.Count)
                        {
                            col = ApCostColors[m.apCost];
                        }
                        options.Add(new TileSelectOption()
                        {
                            Selection = m.destination,
                            OnHover = m.path,
                            HighlightColor = col,
                            HoverColor = hoverColor,
                            ReturnObject = m,
                            OnHoverAction = (
                            () => {
                                if (getHoverEntity() == null)
                                {
                                    profile.PreviewMove(selectedBoardEntity, m);
                                    //profile.UpdateProfile(selectedBoardEntity, displaystats);
                                } }
                            )
                        });
                    }

                    options.Add(new TileSelectOption()
                    {
                        Selection = selectedBoardEntity.GetTile(),
                        HighlightColor = SelectColor,
                        HoverColor = SelectColor,
                        OnHoverAction = (
                            () => {
                                if (getHoverEntity() == null)
                                {
                                    profile.UpdateProfile(selectedBoardEntity);
                                }
                            })
                    });

                    tileSelectionManager.SelectTile(selectedBoardEntity, options, sendMoveToBoardEntity, isMovement: true,
                        hoverExit: () => { if (hoverBoardEntity == null) profile.UpdateProfile(selectedBoardEntity); }
                        );               
                    skillSelector.SetBoardEntity((CharacterBoardEntity)selectedBoardEntity);
                    skillSelector.SetSkills(((CharacterBoardEntity)selectedBoardEntity).Skills);
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
                    setHoverEntity(null);
                    ((CharacterBoardEntity)selectedBoardEntity).ExecuteMove( (Move)tileOption.ReturnObject, (bool ok) => { buildMoveOptions();
                        profile.UpdateProfile(selectedBoardEntity); });
                }
                else
                {
                    if(skillSelector.SelectedSkill == null)
                    {
                        buildMoveOptions();
                    }
                    //((CharacterBoardEntity)selectedBoardEntity).ExecuteMove(null);

                }
                if(skillSelector.SelectedSkill == null)
                {
                    //buildMoveOptions();
                    //setSelectedBoardEntity(null);
                    //skillSelector.Hide();
                }
            }
        }
    }
}
