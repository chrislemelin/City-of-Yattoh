using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Interaction
{
    public class TileSelectionManager : MonoBehaviour
    {
        private Profile profile;
        private Dictionary<Tile, TileSelectOption> possibleTiles = new Dictionary<Tile, TileSelectOption>();

        public bool active = true;
        private bool tileSelectOptionsContainBE = false;
        private bool skillSelectorActive;
        public List<PathOnClick> glowpath = new List<PathOnClick>();

        public Color defaultBoardEntityHighlightColor;
        public Color defaultHighlightColor;
        public Color defaultHoverColor;

        private Action<TileSelectOption> selectionCallBack;
        private BoardEntity selectedEntity;

        public void Init(Profile profile)
        {
            this.profile = profile;
        }

        public bool IsActive()
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
                        unGlow(tempSelectedEntity.GetTile().PathOnClick);
                    }
                 
                    if (pathOnClick != null && possibleTiles.ContainsKey(pathOnClick.Tile))
                    {
                        tempSelectOption(possibleTiles[pathOnClick.Tile]);
                    }
                    else
                    {
                        tempSelectOption(null);
                        active = false;
                        if (pathOnClick != null && tempSelectedEntity != null && pathOnClick != tempSelectedEntity.GetTile().PathOnClick)
                        {
                            pathOnClick.OnMouseUp();
                        }
                        active = true;
                    }
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
                    List<PathOnClick> newPath = new List<PathOnClick>();
                    foreach (Tile t in possibleTiles[pathOnClick.Tile].OnHover)
                    {
                        newPath.Add(t.GetComponentInChildren<PathOnClick>());
                    }
                    if(possibleTiles[pathOnClick.Tile].OnHoverAction != null)
                    {
                        possibleTiles[pathOnClick.Tile].OnHoverAction();
                    }
                    NewGlowPath(newPath);
                }
            }
            else
            {
                NewGlowPath(new List<PathOnClick>());                
            }
        }

        public void ClearTileHover()
        {
            NewGlowPath(new List<PathOnClick>());
            profile.UpdateProfile(selectedEntity);
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

        public void SelectTile (BoardEntity boardEntity, List<Tile> tiles, Action<TileSelectOption> callBack, Color? highlightColor, Color? hoverColor)
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
            SelectTile(boardEntity, options, callBack);
        }

        public void SelectTile(BoardEntity boardEntity, List<Move> moves, Action<TileSelectOption> callBack, Color? highlightColor, Color? hoverColor)
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
            SelectTile(boardEntity, options, callBack);
        }

        public void SelectTile(BoardEntity boardEntity, List<TileSelectOption> options, Action<TileSelectOption> callBack)
        {
            if (selectionCallBack == null)
            {
                selectionCallBack = callBack;

                possibleTiles.Clear();
                options.ForEach(x => possibleTiles.Add(x.Selection, x));

                if (boardEntity != null)
                {
                    selectedEntity = boardEntity;
                    glow(selectedEntity.GetTile().PathOnClick, defaultHoverColor);
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

        private void glow(PathOnClick pathOnClick, Color? overrideColor = null)
        {
            Color? hoverColor = overrideColor;
            if (hoverColor == null)
            {
                hoverColor = defaultBoardEntityHighlightColor;
                if (possibleTiles.ContainsKey(pathOnClick.Tile))
                {
                    hoverColor = possibleTiles[pathOnClick.Tile].HoverColor;
                }
            }
            if(hoverColor != null)
            {
                pathOnClick.ColorEffectManager.TurnOn(this, (Color)hoverColor);
            }
            glowBoardEntity(pathOnClick);
        }

        private void unGlow(PathOnClick tile)
        {
    
            tile.ColorEffectManager.TurnOff(this);
            unGlowBoardEntity(tile);
            
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
            NewGlowPath(new List<PathOnClick>());
        }

        public void NewGlowPath(PathOnClick pathOnClick)
        {
            List<PathOnClick> newPath = new List<PathOnClick>();
            newPath.Add(pathOnClick);
            NewGlowPath(newPath);
        }

        public void NewGlowPath(List<PathOnClick> newPath)
        {
            foreach (PathOnClick item in glowpath)
            {
                unGlow(item);
            }
            foreach (PathOnClick item in newPath)
            {
                glow(item);
            }
            glowpath = newPath;

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
        public bool Clickable = true;

    }
}

