using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSelectManager : MonoBehaviour {

    public bool active = true;
    public bool pause = false;
    public List<PathOnClick> selectedPath = new List<PathOnClick>();


    public Color BoardEntityOnSelectColor;
    public Color TileOnSelectColor;
    public Color MoveableHighlightColor;

    public static PathSelectManager instance;
    public static PathSelectManager Instance
    {
        get { return instance; }
    }

    private BoardEntity selectedBoardEntity;
    private PathOnClick selectedTile = null;
    public PathOnClick SelectedTile
    {
        get { return selectedTile; }
    }

    private Dictionary<Tile, Move> possibleMoves = new Dictionary<Tile, Move>();

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ClearPath()
    {
        NewPath(new List<PathOnClick>());
        updateSelected(null);
    
    }

    public void NewPath(PathOnClick pathOnClick)
    {
        List<PathOnClick> newPath = new List<PathOnClick>();
        newPath.Add(pathOnClick);
        NewPath(newPath);
    }

    public void NewPath(List<PathOnClick> newPath)
    {
        foreach (PathOnClick item in selectedPath)
        {
            unHighlight(item);
            unHighlightBoardEntity(item);
        }
        foreach (PathOnClick item in newPath)
        {
            highlight(item);
            highlightBoardEntity(item);
        }
        selectedPath = newPath;

    }

    public void TileSelected(PathOnClick pathOnClick)
    {

        if (ValidityCheck(pathOnClick))
        {
            if (selectedTile == null)
            {
                NewPath(pathOnClick);
                updateSelected(pathOnClick);
            }
            else if (selectedTile == pathOnClick)
            {
                NewPath(new List<PathOnClick>());
                updateSelected(null);
            }
            else if (selectedBoardEntity == null)
            {
                NewPath(pathOnClick);
                updateSelected(pathOnClick);
            }
            else
            {
                // send the move to the board entity
                if (selectedTile.Tile.BoardEntity != null && isSelectedEnityTurn())
                {
                    selectedTile.Tile.BoardEntity.GetComponent<CharacterBoardEntity>().ExecuteMove(possibleMoves[pathOnClick.Tile]);
                }
            }
        }

    }

    public void TileAltSelected(PathOnClick pathOnClick)
    {
        if(selectedBoardEntity != null)
        {
            //int movementPoints = selectedBoardEntity.State.MovementPoints;
            
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
                foreach (Tile t in possibleMoves[pathOnClick.Tile].path)
                {
                    newPath.Add(t.GetComponentInChildren<PathOnClick>());
                }
                NewPath(newPath);

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
        if(pathOnClick == selectedTile)
        {
            return true;
        }
        // tile is not a valid moveable tile
        if(selectedBoardEntity != null && !possibleMoves.ContainsKey(pathOnClick.Tile))
        {
            return false;
        }
        return true;
    }

    private void updateSelected(PathOnClick newSelected)
    {
        if (selectedTile != newSelected)
        {
            List<Move> newMoves = newSelected != null && newSelected.Tile.BoardEntity != null ?
                newSelected.Tile.BoardEntity.MoveSet() : new List<Move>();
            if (selectedTile != null)
            {
                unHighlightBoardEntity(selectedTile);
                unHighlight(selectedTile);
                foreach (Tile t in possibleMoves.Keys)
                {
                    unHighlightMovableTile(t.GetComponentInChildren<PathOnClick>());
                }
            }
            if (newSelected != null)
            {
                selectedBoardEntity = newSelected.Tile.BoardEntity;
                highlightBoardEntity(newSelected);
                highlight(newSelected);
                // only highlight if it is this character's turn
                if(isSelectedEnityTurn())
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

            Profile.Instance.UpdateProfile(selectedBoardEntity);
            selectedTile = newSelected;
            possibleMoves.Clear();
            newMoves.ForEach(x => possibleMoves.Add(x.destination, x));
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

    private void highlight(PathOnClick pathOnClick)
    {
        pathOnClick.GlowManager.TurnOn(this, TileOnSelectColor);
    }

    private void unHighlight(PathOnClick pathOnClick)
    {
        pathOnClick.GlowManager.TurnOff(this);

    }

    private void highlightBoardEntity(PathOnClick pathOnClick)
    {
        BoardEntity boardEntity = pathOnClick.Tile.BoardEntity;
        if (boardEntity != null)
        {
            boardEntity.GetComponentInChildren<GlowManager>().TurnOn(this, BoardEntityOnSelectColor);

        }

    }

    private void unHighlightBoardEntity(PathOnClick pathOnClick)
    {
        BoardEntity boardEntity = pathOnClick.Tile.BoardEntity;
        if (boardEntity != null)
        {
            boardEntity.GetComponentInChildren<GlowManager>().TurnOff(this);
        }
    }

    private bool isSelectedEnityTurn()
    {
        return (selectedBoardEntity != null && selectedBoardEntity == TurnManager.Instance.CurrentBoardEntity);
    }

}
