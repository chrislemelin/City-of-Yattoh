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
        private Dictionary<Tile, Move> possibleMoves = new Dictionary<Tile, Move>();

        public bool active = true;
        public bool pause = false;
        public List<PathOnClick> glowpath = new List<PathOnClick>();

        public Color BoardEntityOnSelectColor;
        public Color TileOnSelectColor;
        public Color MoveableHighlightColor;

        private Action<Move> moveCallBack;

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

        public void ClearGlowPath()
        {
            NewGlowPath(new List<PathOnClick>());
            if (selectedTile != null)
            {
                unHighlight(selectedTile);
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
                unHighlight(item);
            }
            foreach (PathOnClick item in newPath)
            {
                highlight(item);
            }
            glowpath = newPath;

        }

        public void TileClicked(PathOnClick pathOnClick)
        {

            if (moveCallBack != null)
            {
                if (possibleMoves.ContainsKey(pathOnClick.Tile))
                {
                    moveCallBack(possibleMoves[pathOnClick.Tile]);
                }
                else
                {
                    moveCallBack(null);
                }

                foreach (Tile t in possibleMoves.Keys)
                {
                    unHighlightMovableTile(t.GetComponentInChildren<PathOnClick>());
                }


                ClearGlowPath();
                moveCallBack = null;
                selectedBoardEntity = null;
                selectedTile = null;
                possibleMoves.Clear();

            }

            /*
            if (ValidityCheck(pathOnClick))
            {
                // we dont have anything previously selected
                if (selectedTile == null)
                {
                    NewGlowPath(pathOnClick);
                    updateSelected(pathOnClick);
                }
                // clicking selected thing
                else if (selectedTile == pathOnClick)
                {
                    NewGlowPath(new List<PathOnClick>());
                    updateSelected(null);
                }
                // see if the selected tile has a board entity in it
                else if (selectedBoardEntity == null)
                {
                    NewGlowPath(pathOnClick);
                    updateSelected(pathOnClick);
                }
                // send the selected move to the board entity
                else
                {
                    if(moveCallBack != null)
                    {
                        moveCallBack(possibleMoves[pathOnClick.Tile]);
                    }
                    // send the move to the board entity
                    if (selectedTile.Tile.BoardEntity != null && isSelectedEnityTurn())
                    {
                        selectedTile.Tile.BoardEntity.GetComponent<CharacterBoardEntity>().ExecuteMove(possibleMoves[pathOnClick.Tile]);
                    }
                }
            }
            */
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
                    foreach (Tile t in possibleMoves[pathOnClick.Tile].path)
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
            if (selectedBoardEntity != null && !possibleMoves.ContainsKey(pathOnClick.Tile))
            {
                return false;
            }
            return true;
        }

        private void updateSelected(PathOnClick newSelected)
        {
            if (selectedTile == newSelected)
            {
                List<Move> newMoves = newSelected != null && newSelected.Tile.BoardEntity != null ?
                    newSelected.Tile.BoardEntity.MoveSet() : new List<Move>();
                if (selectedTile != null)
                {
                    unHighlight(selectedTile);
                    foreach (Tile t in possibleMoves.Keys)
                    {
                        unHighlightMovableTile(t.GetComponentInChildren<PathOnClick>());
                    }
                }
                if (newSelected != null)
                {
                    selectedBoardEntity = newSelected.Tile.BoardEntity;
                    highlight(newSelected);
                    if (isSelectedEnityTurn())
                    {
                        foreach (Move t in newMoves)
                        {
                            highlightMovableTile(t.destination.GetComponentInChildren<PathOnClick>());
                        }
                    }
                }
                else
                {
                    selectedBoardEntity = null;
                }

                profile.UpdateProfile(selectedBoardEntity);
                selectedTile = newSelected;
                possibleMoves.Clear();
                newMoves.ForEach(x => possibleMoves.Add(x.destination, x));
            }

        }

        public void SelectTile(BoardEntity boardEntity, List<Move> moves, Action<Move> callBack)
        {
            if (moveCallBack == null)
            {
                moveCallBack = callBack;
                selectedBoardEntity = boardEntity.GetTile().BoardEntity;
                highlight(boardEntity.GetTile().PathOnClick);

                // highlight possible moves
                foreach (Move t in moves)
                {
                    highlightMovableTile(t.destination.GetComponentInChildren<PathOnClick>());
                }


                profile.UpdateProfile(selectedBoardEntity);
                selectedTile = boardEntity.GetTile().PathOnClick;
                possibleMoves.Clear();
                moves.ForEach(x => possibleMoves.Add(x.destination, x));
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

        private void highlight(PathOnClick tile)
        {
            tile.GlowManager.TurnOn(this, TileOnSelectColor);
            highlightBoardEntity(tile);
        }

        private void unHighlight(PathOnClick tile)
        {
            tile.GlowManager.TurnOff(this);
            unHighlightBoardEntity(tile);

        }

        private void highlightBoardEntity(PathOnClick tile)
        {
            BoardEntity boardEntity = tile.Tile.BoardEntity;
            if (boardEntity != null)
            {
                boardEntity.GetComponentInChildren<GlowManager>().TurnOn(this, BoardEntityOnSelectColor);

            }

        }

        private void unHighlightBoardEntity(PathOnClick tile)
        {
            BoardEntity boardEntity = tile.Tile.BoardEntity;
            if (boardEntity != null)
            {
                boardEntity.GetComponentInChildren<GlowManager>().TurnOff(this);
            }
        }

        private bool isSelectedEnityTurn()
        {
            return (selectedBoardEntity != null && selectedBoardEntity == turnManager.CurrentBoardEntity);
        }
    }
}

