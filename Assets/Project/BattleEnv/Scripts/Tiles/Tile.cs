using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Interaction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Placeholdernamespace.Battle.Env
{
    public class Tile : MonoBehaviour
    {
        public LayerMask myLayerMask;
        public GameObject trap;
        private GameObject currentTrap;

        private List<TileListener> tileListeners = new List<TileListener>();
        public  List<TileListener> TileListeners
        {
            get { return new List<TileListener>(tileListeners); }
        }           

        private TileManager tileManager = null;
        public bool canRemove = false;

        private Position position;
        public Position Position
        {
            get { return position; }
        }

        private PathOnClick pathOnClick;
        public PathOnClick PathOnClick
        {
            get { return GetComponentInChildren<PathOnClick>(); }
        }

        private BoardEntity boardEntity;
        public BoardEntity BoardEntity
        {
            get { return boardEntity; }
        }

        public void SetBoardEntity(BoardEntity value)
        {
            boardEntity = value;
        }

        public delegate void TileEnterAction(BoardEntity boardEntity, Tile tile, Action callback);
        private Dictionary<Tile, bool> checkIfBlockedCaching = new Dictionary<Tile, bool>();

        public void Init(Position position, TileManager tileManager)
        {
            this.position = position;
            this.tileManager = tileManager;
        }

        public void ClearBlockingCache()
        {
            checkIfBlockedCaching.Clear();
        }

        public bool CheckIfBlocked(Tile tile)
        {
            /*
            if(checkIfBlockedCaching.ContainsKey(tile))
            {
                return checkIfBlockedCaching[tile];
            }
            Vector2 currentPos = transform.position;
            Vector2 tilePos = tile.transform.position;
            float distance = Vector2.Distance(currentPos, tilePos);
            Vector2 rayDirection = tilePos - currentPos;
            */
            return false;
            /* performance problem here
            RaycastHit2D hit = Physics2D.Raycast(currentPos, rayDirection, distance, myLayerMask);   
            if(hit.transform == null)
            {
                checkIfBlockedCaching.Add(tile, false);
                return false;
            }
            else
            {
                checkIfBlockedCaching.Add(tile, true);
                return true;
            }
            */
        }

        public Tile GetTileInReferenceTo(Position offset)
        {
            return tileManager.GetTile(Position + offset).GetComponent<Tile>();
        }

        public BoardEntity GetInReferencTo(Position offset)
        {
            return tileManager.GetTile(Position + offset).GetComponent<Tile>().boardEntity;
        }

        public Tile GetLeftNeighborTile()
        {
            return GetTileInReferenceTo(new Position(-1, 0));
        }

        public Tile GetRightNeighborTile()
        {
            return GetTileInReferenceTo(new Position(1, 0));
        }

        public Tile GetUpperNeighborTile()
        {
            return GetTileInReferenceTo(new Position(0, 1));
        }

        public Tile GetLowerNeighborTile()
        {
            return GetTileInReferenceTo(new Position(0, -1));
        }

        public BoardEntity GetLeftNeighborBoardEntity()
        {
            return GetLeftNeighborTile().GetComponent<Tile>().BoardEntity;
        }

        public BoardEntity GetRightNeighborBoardEntity()
        {
            return GetRightNeighborTile().GetComponent<Tile>().BoardEntity;
        }

        public BoardEntity GetUpperNeighborBoardEntity()
        {
            return GetUpperNeighborTile().GetComponent<Tile>().BoardEntity;
        }

        public BoardEntity GetLowerNeighborBoardEntity()
        {
            return GetLowerNeighborTile().GetComponent<Tile>().BoardEntity;
        }

        public List<Tile> GetAllTilesNear(Position range, bool ignoreWalls = false)
        {
            return tileManager.GetTilesDiag(Position, range, ignoreWalls);
        }

        public List<Tile> GetAllTilesNear(int range = 1, bool ignoreWalls = false)
        {
            return tileManager.GetTilesDiag(Position, range, ignoreWalls);
        }

        public List<BoardEntity> GetAllNear(Position range, bool ignoreWalls = false)
        {
            return tileManager.GetBoardEntityDiag(Position, range, ignoreWalls);
        }

        public List<BoardEntity> GetAllNear(int range = 1, bool ignoreWalls = false)
        {
            return tileManager.GetBoardEntityDiag(Position, range, ignoreWalls);
        }

        public void ExecuteEnterActions(BoardEntity boardEntity, Tile t, Action callback)
        {
            enterActionCounter = 0;
            enterBoardEntity = boardEntity;
            enterActionCallback = callback;
            leavingTile = t;
            ExecuteEnterActionsHelper();
                      
        }

        public void AddTileListener(TileListener tileListener)
        {
            tileListener.Init(tileListeners.Remove);
            tileListeners.Add(tileListener);
        }

        private List<TileEnterAction> enterActions = new List<TileEnterAction>();
        private int enterActionCounter = 0;
        private BoardEntity enterBoardEntity = null;
        private Action enterActionCallback;
        private Tile leavingTile;

        private void ExecuteEnterActionsHelper()
        {
            if (enterActionCounter < enterActions.Count)
            {
                int tempEnterActionCounter = enterActionCounter;
                enterActionCounter++;
                enterActions[tempEnterActionCounter](enterBoardEntity, leavingTile, ExecuteEnterActionsHelper);
            }
            else
            {
                if(enterActionCallback != null)
                {
                    enterActionCallback();
                }
            }
        }

        public void RemoveEnterAction(TileEnterAction action)
        {
            enterActions.Remove(action);
        }

        public void AddEnterActions(TileEnterAction action)
        {
            enterActions.Add(action);
        }
    }
}
