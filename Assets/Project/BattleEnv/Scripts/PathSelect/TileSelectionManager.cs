using Placeholdernamespace.Battle.Entities;
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
        private TurnManager turnManager;
        private Profile profile;
        private Dictionary<Tile, TileSelectOption> possibleTiles = new Dictionary<Tile, TileSelectOption>();

        public bool active = true;
        public bool pause = false;
        public List<PathOnClick> glowpath = new List<PathOnClick>();

        public Color BoardEntityOnSelectColor;
        public Color TileOnSelectColor;
        public Color MoveableHighlightColor;

        private Action<Tile> selectionCallBack;

        private BoardEntity selectedBoardEntity;
        public BoardEntity SelectedBoardEntity
        {
            get { return selectedBoardEntity; }
        }

        private PathOnClick selectedTile = null;
        public PathOnClick SelectedTile
        {
            get { return selectedTile; }
        }


        public void Init(TurnManager turnManager, Profile profile)
        {
            this.turnManager = turnManager;
            this.profile = profile;
        }


        public void TileClicked(PathOnClick pathOnClick)
        {

            if (selectionCallBack != null)
            {
                if (possibleTiles.ContainsKey(pathOnClick.Tile))
                {
                    selectionCallBack(pathOnClick.Tile);
                }
                else
                {
                    selectionCallBack(null);
                }

                foreach (Tile t in possibleTiles.Keys)
                {
                    unHighlightMovableTile(t.GetComponentInChildren<PathOnClick>());
                }


                ClearGlowPath();
                selectionCallBack = null;
                selectedBoardEntity = null;
                selectedTile = null;
                possibleTiles.Clear();

            }
        }

        public void TileHover(PathOnClick pathOnClick)
        {
            if (ValidityCheck(pathOnClick))
            {
                if (pathOnClick == selectedTile)
                {
                    return;
                }

                if (selectedTile.Tile.BoardEntity != null)
                {
                    List<PathOnClick> newPath = new List<PathOnClick>();
                    foreach (Tile t in possibleTiles[pathOnClick.Tile].OnHover)
                    {
                        newPath.Add(t.GetComponentInChildren<PathOnClick>());
                    }
                    NewGlowPath(newPath);
                }
            }
        }

        private bool ValidityCheck(PathOnClick pathOnClick)
        {
            // on pause
            if (pause)
            {
                return false;
            }
            // should always be able to deselect
            if (pathOnClick == selectedTile)
            {
                return true;
            }
            // tile is not a valid moveable tile
            if (selectedBoardEntity != null && !possibleTiles.ContainsKey(pathOnClick.Tile))
            {
                return false;
            }
            return true;
        }

        public void SelectTile(BoardEntity boardEntity, List<Move> moves, Action<Tile> callBack)
        {
            List<TileSelectOption> options = new List<TileSelectOption>();
            foreach(Move m in moves)
            {
                options.Add(new TileSelectOption()
                {
                    Selection = m.destination,
                    OnHover = m.path                   
                });
            }
            SelectTile(boardEntity, options, callBack);
        }

        public void SelectTile(BoardEntity boardEntity, List<TileSelectOption> options, Action<Tile> callBack)
        {
            if (selectionCallBack == null)
            {
                selectionCallBack = callBack;
                selectedBoardEntity = boardEntity.GetTile().BoardEntity;
                glow(boardEntity.GetTile().PathOnClick);

                // highlight possible moves
                foreach (TileSelectOption opt in options)
                {
                    highlightMovableTile(opt.Selection.GetComponentInChildren<PathOnClick>());
                }

                profile.UpdateProfile(selectedBoardEntity);
                selectedTile = boardEntity.GetTile().PathOnClick;
                possibleTiles.Clear();
                options.ForEach(x => possibleTiles.Add(x.Selection, x));
            }
        }

        private void highlightMovableTile(PathOnClick pathOnClick)
        {
            pathOnClick.ColorEffectManager.TurnOn(this, MoveableHighlightColor);
        }

        private void unHighlightMovableTile(PathOnClick pathOnClick)
        {
            pathOnClick.ColorEffectManager.TurnOff(this);
        }

        private void glow(PathOnClick tile)
        {
            tile.GlowManager.TurnOn(this, TileOnSelectColor);
            glowBoardEntity(tile);
        }

        private void unGlow(PathOnClick tile)
        {
            tile.GlowManager.TurnOff(this);
            unGlowBoardEntity(tile);

        }

        private void glowBoardEntity(PathOnClick tile)
        {
            BoardEntity boardEntity = tile.Tile.BoardEntity;
            if (boardEntity != null)
            {
                boardEntity.GetComponentInChildren<GlowManager>().TurnOn(this, BoardEntityOnSelectColor);
            }
        }

        private void unGlowBoardEntity(PathOnClick tile)
        {
            BoardEntity boardEntity = tile.Tile.BoardEntity;
            if (boardEntity != null)
            {
                boardEntity.GetComponentInChildren<GlowManager>().TurnOff(this);
            }
        }


        public void ClearGlowPath()
        {
            NewGlowPath(new List<PathOnClick>());
            if (selectedTile != null)
            {
                unGlow(selectedTile);
            }

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

        private bool isSelectedEnityTurn()
        {
            return (selectedBoardEntity != null && selectedBoardEntity == turnManager.CurrentBoardEntity);
        }
    }

    public class TileSelectOption
    {
        public Tile Selection;
        public List<Tile> OnHover;
    }
}

