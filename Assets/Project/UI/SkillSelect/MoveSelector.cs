using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.UI
{
    public class MoveSelector : BaseSelector
    {
        [SerializeField]
        Color SelectColor;

        [SerializeField]
        Profile profile;

        [SerializeField]
        private List<Color> ApCostColors;

        [SerializeField]
        private Color hoverColor;

        [SerializeField]
        private CharacterBoardEntity selectedBoardEntity = null;
        private TileSelectionManager tileSelectionManager;

        [SerializeField]
        private BoardEntitySelector boardEntitySelector;

        private Action doneAction; 

        public void Init(TileSelectionManager tileSelectionManager, Transform targetTransform, GameObject parent)
        {
            this.tileSelectionManager = tileSelectionManager;
            base.Init(targetTransform, parent);
        }

        public void SetDoneAction(Action doneAction)
        {
            this.doneAction = () => { ClearButtons(); profile.UpdateProfile(selectedBoardEntity); tileSelectionManager.CancelSelection(); doneAction(); };

        }

        public List<TileSelectOption> GetTileSelectOptions(CharacterBoardEntity characterBoardEntity, bool preview = false, int? ap = null)
        {
            List<Move> moveSet = characterBoardEntity.MoveSet(ap);
            List<TileSelectOption> options = new List<TileSelectOption>();
            TileSelectOption newOption;
            foreach (Move m in moveSet)
            {
                Stats displaystats = characterBoardEntity.Stats.GetCopy();
                displaystats.SubtractAPPoints(m.apCost);
                displaystats.SetMutableStat(StatType.Movement, m.movementPointsAfterMove);
                Color col = ApCostColors[0];
                if (m.apCost < ApCostColors.Count)
                {
                    col = ApCostColors[m.apCost];
                }
                newOption = new TileSelectOption()
                {
                    Selection = m.destination,
                    OnHover = m.path,
                    HighlightColor = col,
                    HoverColor = hoverColor,
                    ReturnObject = m,
                    OnHoverAction = (
                    () => { profile.PreviewMove(characterBoardEntity, m); })
                };
                if(preview)
                {
                    newOption.OnHoverAction = null;
                    newOption.OnHover = null;
                }
                options.Add(newOption);
            }
            newOption = new TileSelectOption()
            {
                Selection = characterBoardEntity.GetTile(),
                HighlightColor = SelectColor,
                HoverColor = SelectColor,
                OnHoverAction = () => profile.UpdateProfile(characterBoardEntity)
            };
            if(preview)
            {
                newOption.OnHoverAction = null;
            }
            options.Add(newOption);

            return options;
        }

        public void BuildPreviewMoveOptions(CharacterBoardEntity selectedBoardEntity)
        {
            tileSelectionManager.CancelSelection();
            if (selectedBoardEntity != null)
            {
                List<TileSelectOption> options = GetTileSelectOptions(selectedBoardEntity, true, 2);
                tileSelectionManager.SelectTile(selectedBoardEntity, options, PreviewAction, isMovement: true);
            }

        }

        private void PreviewAction(TileSelectOption tileOption)
        {
            boardEntitySelector.SetPreviewBoardEntity(null);
        }

        public void BuildMoveOptions(CharacterBoardEntity selectedBoardEntity)
        {      
            buildCancelSkillButton(()=> sendMoveToBoardEntity(null));
            this.selectedBoardEntity = selectedBoardEntity;
            if (TurnManager.CurrentBoardEntity == selectedBoardEntity)
            {
                List<Move> moveSet = selectedBoardEntity.MoveSet();
                List<TileSelectOption> options = GetTileSelectOptions(selectedBoardEntity);

                tileSelectionManager.SelectTile(selectedBoardEntity, options, sendMoveToBoardEntity, isMovement: true,
                    hoverExit: () => {profile.UpdateProfile(selectedBoardEntity); }
                    );
            }
            else
            {
                tileSelectionManager.SelectTile(selectedBoardEntity, new List<Move>(), sendMoveToBoardEntity, null, null);
            }
            
        }

        private void sendMoveToBoardEntity(TileSelectOption tileOption)
        {
            SetInteractableCancelButton(false);
            if (tileOption != null)
            {
                selectedBoardEntity.ExecuteMove((Move)tileOption.ReturnObject, (bool ok) =>
                {
                    doneAction();
                    profile.UpdateProfile(selectedBoardEntity);
                });
            }
            else
            {
                doneAction();
            }
        }
        

    }
}
