using Placeholdernamespace.Battle.Entities;
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

        private TileManager tileManager = null;

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
            set { boardEntity = value; }
        }

        public delegate void TileEnterAction(BoardEntity boardEntity, Tile tile, Action callback);

        public void Init(Position position, TileManager tileManager)
        {
            this.position = position;
            this.tileManager = tileManager;
        }

        public void SetBoardEntity(BoardEntity boardEntity)
        {
            this.boardEntity = boardEntity;
        }

        public bool CheckIfBlocked(Tile tile)
        {
            Vector2 currentPos = transform.position;
            Vector2 tilePos = tile.transform.position;
            float distance = Vector2.Distance(currentPos, tilePos);
            Vector2 rayDirection = tilePos - currentPos;

            RaycastHit2D hit = Physics2D.Raycast(currentPos, rayDirection, distance, myLayerMask);   
            if(hit.transform == null)
            {
                return false;
            }
            else
            {
                return true;
            }
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
