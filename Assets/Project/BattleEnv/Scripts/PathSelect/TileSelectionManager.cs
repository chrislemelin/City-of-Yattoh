using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Interaction
{
    public class 
        TileSelectionManager : MonoBehaviour
    {
        private Profile profile;
        private Dictionary<Tile, TileSelectOption> possibleTiles = new Dictionary<Tile, TileSelectOption>();

        public bool active = true;
        private bool tileSelectOptionsContainBE = false;
        private bool skillSelectorActive;
        public List<Tile> glowpath = new List<Tile>();
        private bool isMovement = false;

        public Color defaultBoardEntityHighlightColor;
        public Color defaultHighlightColor;
        public Color defaultHoverColor;

        private Action<TileSelectOption> selectionCallBack;
        private Action hoverExit;
        private BoardEntity selectedEntity;

        public void Init(Profile profile)
        {
            this.profile = profile;
        }

        public bool IsActive()
        {
            if(selectionCallBack != null && isMovement)
            {
                return false;
            }
            return selectionCallBack != null;
        }

        public bool IsActiveHover()
        {
            return selectionCallBack != null;
        }

        public void TileClicked(PathOnClick pathOnClick)
        {
            if (selectionCallBack != null && active)
            {              
                //checks to see if the selected tile is unselectable
                if (!(pathOnClick != null && possibleTiles.ContainsKey(pathOnClick.Tile) && !possibleTiles[pathOnClick.Tile].Clickable))
                {
                    Action<TileSelectOption> tempSelectOption = selectionCallBack;
                    selectionCallBack = null;
                    BoardEntity tempSelectedEntity = selectedEntity;
                    selectedEntity = null;
                    ClearGlowPath();

                    foreach (Tile t in possibleTiles.Keys)
                    {
                        unHighlightMovableTile(t.GetComponentInChildren<PathOnClick>());
                    }
                    if (tempSelectedEntity != null)
                    {
                        unGlow(tempSelectedEntity.GetTile());
                    }
                    
                    if (pathOnClick != null && possibleTiles.ContainsKey(pathOnClick.Tile))
                    {
                        tempSelectOption(possibleTiles[pathOnClick.Tile]);
                    }
                    else
                    {
                        tempSelectOption(null);
                        active = false;
                        if (pathOnClick != null && tempSelectedEntity != null && pathOnClick != tempSelectedEntity.GetTile().PathOnClick && !isMovement)
                        {
                            //pathOnClick.OnMouseUp();
                        }
                        active = true;
                    }
                    isMovement = false;
                }
            }
        }

        public void CancelSelection()
        {
            //profile.UpdateProfile(selectedEntity);
            TileClicked(null);
        }

        public void TileHover(PathOnClick pathOnClick)
        {
            if (ValidityCheck(pathOnClick))
            {
                if (possibleTiles.ContainsKey(pathOnClick.Tile))
                {
                    NewGlowPath(possibleTiles[pathOnClick.Tile]);
                    if(possibleTiles[pathOnClick.Tile].OnHoverAction != null)
                    {
                        possibleTiles[pathOnClick.Tile].OnHoverAction();
                    }
                    
                }
            }
            else
            {
                NewGlowPath(null);     
                if(hoverExit != null)
                {
                    hoverExit();
                }
                if(pathOnClick.Tile.BoardEntity != null)
                {
                    profile.UpdateProfile(pathOnClick.Tile.BoardEntity);
                }
            }
        }

        public void ClearTileHover()
        {
            NewGlowPath(null);
            //profile.UpdateProfile(selectedEntity);
        }

        private bool ValidityCheck(PathOnClick pathOnClick)
        {
            // tile is not a valid moveable tile
            if (selectionCallBack != null && !possibleTiles.ContainsKey(pathOnClick.Tile))
            {
                return false;
            }
            return true;
        }

        public void SelectTile (BoardEntity boardEntity, List<Tile> tiles, Action<TileSelectOption> callBack, Color? highlightColor, Color? hoverColor, 
            bool isMovement = false, Action HoverExit = null)
        {
            Color highlightColorUsing = (Color)(highlightColor != null ? highlightColor : defaultHighlightColor);
            Color hoverColorUsing = (Color)(hoverColor != null ? hoverColor : defaultHoverColor);

            List<TileSelectOption> options = new List<TileSelectOption>();
            foreach (Tile t in tiles)
            {
                options.Add(new TileSelectOption()
                {
                    Selection = t,
                    OnHover = new List<Tile>() { t },
                    HighlightColor = highlightColorUsing,
                    HoverColor = hoverColorUsing
                });
            }
            SelectTile(boardEntity, options, callBack, hoverExit:hoverExit);
        }

        public void SelectTile(BoardEntity boardEntity, List<Move> moves, Action<TileSelectOption> callBack, Color? highlightColor, Color? hoverColor,
            bool isMovement = false, Action hoverExit = null)
        {
            Color highlightColorUsing = (Color)(highlightColor!= null ? highlightColor: defaultHighlightColor);
            Color hoverColorUsing = (Color)(hoverColor != null ? hoverColor : defaultHoverColor);

            List<TileSelectOption> options = new List<TileSelectOption>();
            foreach(Move m in moves)
            {
                options.Add(new TileSelectOption()
                {
                    Selection = m.destination,
                    OnHover = m.path,
                    HighlightColor = highlightColorUsing,
                    HoverColor = hoverColorUsing,
                    ReturnObject = m
                });
            }
            SelectTile(boardEntity, options, callBack, isMovement, hoverExit:hoverExit);
        }

        // this is the main one
        public void SelectTile(BoardEntity boardEntity, List<TileSelectOption> options, Action<TileSelectOption> callBack, bool isMovement = false,
             Action hoverExit = null)
        {
            this.hoverExit = hoverExit;
            if (selectionCallBack == null)
            {
                this.isMovement = isMovement;
                selectionCallBack = callBack;

                possibleTiles.Clear();
                options.ForEach(x => possibleTiles.Add(x.Selection, x));

                if (boardEntity != null)
                {
                    selectedEntity = boardEntity;
                    glow(selectedEntity.GetTile(), defaultHoverColor);
                }

                // highlight possible moves
                foreach (TileSelectOption opt in options)
                {
                    highlightMovableTile(opt.Selection.GetComponentInChildren<PathOnClick>());
                }

                //selectedTile = boardEntity.GetTile().PathOnClick;
 
            }
        }

        private void highlightMovableTile(PathOnClick pathOnClick)
        {
            Color? highlightColor = defaultHighlightColor;
            if (possibleTiles.ContainsKey(pathOnClick.Tile))
            {
                highlightColor = possibleTiles[pathOnClick.Tile].HighlightColor;
            }
            if(highlightColor != null)
            {
                pathOnClick.ColorEffectManager.TurnOn(this, (Color)highlightColor);
            }
        }

        private void unHighlightMovableTile(PathOnClick pathOnClick)
        {
            if (possibleTiles[pathOnClick.Tile].HighlightColor != null)
            {
                pathOnClick.ColorEffectManager.TurnOff(this);
            }
        }

        private void glow(Tile tile, Color? overrideColor = null)
        {
            Color? hoverColor = overrideColor;
            if (hoverColor == null)
            {
                hoverColor = defaultBoardEntityHighlightColor;
       
            }
            if(hoverColor != null)
            {
                tile.PathOnClick.ColorEffectManager.TurnOn(this, (Color)hoverColor);
            }
            //glowBoardEntity(pathOnClick);
        }

        private void unGlow(Tile tile)
        {
    
            tile.PathOnClick.ColorEffectManager.TurnOff(this);
            //unGlowBoardEntity(tile);
            
        }

        private void glowBoardEntity(PathOnClick tile)
        {
            BoardEntity boardEntity = tile.Tile.BoardEntity;
            if (boardEntity != null)
            {
                //boardEntity.GetComponentInChildren<GlowManager>().TurnOn(this, defaultBoardEntityHighlightColor);
            }
        }

        public void GlowBoardEntity(BoardEntity boardEntity)
        {
            if (boardEntity != null)
            {                
                //boardEntity.GetComponentInChildren<GlowManager>().TurnOn(this, defaultBoardEntityHighlightColor);
            }
        }

        private void unGlowBoardEntity(PathOnClick tile)
        {
            BoardEntity boardEntity = tile.Tile.BoardEntity;
            if (boardEntity != null)
            {
                //boardEntity.GetComponentInChildren<GlowManager>().TurnOff(this);
            }
        }

        public void UnGlowBoardEntity(BoardEntity boardEntity)
        {
            if (boardEntity != null)
            {
                //boardEntity.GetComponentInChildren<GlowManager>().TurnOff(this);
            }
        }

        public void ClearGlowPath()
        {
            NewGlowPath(null);
        }

        public void NewGlowPath(TileSelectOption tileSelectOption)
        {
            List<Tile> newTiles = new List<Tile>();
            if(tileSelectOption != null)
            {
                newTiles = tileSelectOption.OnHover;
            }

            foreach (Tile tile in glowpath)
            {
                unGlow(tile);
            }
            if(glowpath.Count> 0)
            {
                glowpath[glowpath.Count-1].PathOnClick.TurnHoverOff();
            }
            foreach (Tile tile in newTiles)
            {
                glow(tile);
            }
            glowpath = newTiles;

        }

    }

    public class TileSelectOption
    {
        
        public Tile Selection;
        public List<Tile> OnHover = new List<Tile>();
        public Color? HoverColor;
        public Color? HighlightColor;
        public object ReturnObject;
        public Action OnHoverAction;
        public Stats DisplayStats;
        public SkillReport skillReport;
        public bool Clickable = true;

    }
}

