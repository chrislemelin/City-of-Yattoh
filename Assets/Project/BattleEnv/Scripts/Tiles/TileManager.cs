using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Placeholdernamespace.Battle.Env
{
    public class TileManager : MonoBehaviour
    {

        public BoardEntitySelector boardEntitySelector;
        public TileGenerator generator;

        private TurnManager turnManager;
        private Profile profile;

        private Dictionary<Tuple<Position, Position>, GameObject> tupleToWall = new Dictionary<Tuple<Position, Position>, GameObject>();
        private Dictionary<Position, Tile> coordinateToTile = new Dictionary<Position, Tile>();
        private Dictionary<Position, Tile> tempCoordinateToTile = null;

        public event Action UpdatedBoardEntityPosition;

        public void Init(TurnManager turnManager, Profile profile)
        {
            this.turnManager = turnManager;
            this.profile = profile;
            this.boardEntitySelector.Init();
            generateBoard();
        }

        public void MoveBoardEntity(Position p, BoardEntity entity)
        {
            Tile oldTile = entity.GetTile();
            Tile tile = GetTile(p);      

            entity.GetTile().BoardEntity = null;
            entity.Position = p;
            entity.GetTile().BoardEntity = entity;
            entity.transform.position = entity.GetTile().transform.position;

            if (entity is CharacterBoardEntity)
            {
                List<Passive> passivesTemp = ((CharacterBoardEntity)entity).Passives;
                foreach (Passive passive in passivesTemp)
                {
                    passive.LeaveTile(oldTile);
                    passive.EnterTile(tile);
                }
            }
            if (UpdatedBoardEntityPosition != null)
                UpdatedBoardEntityPosition();
        }

        public Tile GetTile(Position position)
        {
            if(position == null)
            {
                int a = 0;
            }
            Dictionary<Position, Tile> coords;
            if(tempCoordinateToTile != null)
            {
                coords = tempCoordinateToTile;
            }
            else
            {
                coords = coordinateToTile;
            }

            if (coords.ContainsKey(position))
            {
                return coords[position];
            }
            else
            {
                return null;
            }

        }

        public GameObject GetWall(Position position1, Position position2)
        {
            GameObject returnObj;
            Tuple<Position, Position> firstCombo = new Tuple<Position, Position>(position1, position2);
            Tuple<Position, Position> secondCombo = new Tuple<Position, Position>(position2, position1);

            if (tupleToWall.TryGetValue(firstCombo, out returnObj))
            {
                return returnObj;
            }
            else if (tupleToWall.TryGetValue(secondCombo, out returnObj))
            {
                return returnObj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// counts a diagonal move as 2 cost
        /// </summary>
        /// <param name="position"></param>
        /// <param name="range"></param>
        /// <param name="ignoreWalls"></param>
        /// <returns></returns>
        public List<Tile> GetTilesNoDiag(Position position, int range, bool ignoreWalls = false, bool lineOfSight = true)
        {
            Tile startingTile = GetTile(position);
            HashSet<Tile> returnTiles = new HashSet<Tile>();
            HashSet<Tile> processingTiles = new HashSet<Tile>();
            processingTiles.Add(GetTile(position));
            for(int a = 0; a < range; a ++)
            {
                HashSet<Tile> newProcessingTiles = new HashSet<Tile>();
                foreach(Tile t in processingTiles)
                {
                    List<Tile> newTiles = GetTilesNoDiag(t.Position, ignoreWalls);
                    foreach(Tile newTile in newTiles)
                    {
                        if (!returnTiles.Contains(newTile) && GetTile(position)!= newTile)
                        {
                            if(!lineOfSight || !CheckIfBlocked(startingTile.Position, newTile.Position))
                            {
                                newProcessingTiles.Add(newTile);
                                returnTiles.Add(newTile);
                            }

                        }
                    }
                    
                }
                processingTiles = newProcessingTiles;
            }
            return returnTiles.ToList();


        }

        /// <summary>
        /// counts a diagonal move as 2 cost
        /// </summary>
        /// <param name="position"></param>
        /// <param name="ignoreWalls"></param>
        /// <returns></returns>
        public List<Tile> GetTilesNoDiag(Position position, bool ignoreWalls = false)
        {
            Tile startingTile = GetTile(position);
            List<Tile> returnList = new List<Tile>();
            Tile neighbor;

            neighbor = GetTile(position + new Position(1, 0));
            addTileHelper(returnList, neighbor, startingTile, ignoreWalls);

            neighbor = GetTile(position + new Position(-1, 0));
            addTileHelper(returnList, neighbor, startingTile, ignoreWalls);

            neighbor = GetTile(position + new Position(0, 1));
            addTileHelper(returnList, neighbor, startingTile, ignoreWalls);

            neighbor = GetTile(position + new Position(0, -1));
            addTileHelper(returnList, neighbor, startingTile, ignoreWalls);

            return returnList;
        }

        private void addTileHelper(List<Tile> returnList, Tile tile, Tile startingTile ,bool ignoreWalls)
        {
            if (tile != null)
            {
                if (ignoreWalls)
                {
                    returnList.Add(tile);
                }
                else if (!startingTile.CheckIfBlocked(tile))
                {
                    returnList.Add(tile);
                }
            }
        }

        /// <summary>
        /// moving on the diagonal cost 1 ap
        /// </summary>
        /// <param name="position"></param>
        /// <param name="range"></param>
        /// <param name="ignoreWalls"></param>
        /// <param name="sortByDistance"></param>
        /// <returns></returns>
        public List<Tile> GetTilesDiag(Position position, Position range, bool lineOfSight = false,
            bool ignoreWalls = false, bool sortByDistance = false)
        {

            Tile startingTile = GetTile(position);
            List<Tile> returnList = new List<Tile>();
            for (int x = -range.x; x <= range.x; x++)
            {
                for (int y = -range.y; y <= range.y; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    Position neighborVector = new Position(x + position.x, y + position.y);
                    Tile neighbor = this.GetTile(neighborVector);

                    if (neighbor != null)
                    {
                        if(!lineOfSight || !CheckIfBlocked(startingTile.Position, neighbor.Position))
                            returnList.Add(neighbor);

                    }
                }
            }
            if (sortByDistance)
            {
                //float dist = position.GetDistance()
                returnList = returnList.OrderBy(x => x.Position.GetDistance(position)).ToList<Tile>();
            }
            return returnList;
        }

        public List<Tile> GetTilesDiag(Position position, int range = 1, bool ignoreWalls = false, bool sortByDistance = false)
        {
            return GetTilesDiag(position, new Position(range, range), ignoreWalls, sortByDistance);
        }

        public List<BoardEntity> GetBoardEntityDiag(Position position, Position Range, bool ignoreWalls = false, bool sortByDistance = false)
        {
            List<Tile> tiles = GetTilesDiag(position, Range, ignoreWalls, sortByDistance);
            return TilesToBoardEntities(tiles);
        }

        public List<BoardEntity> GetBoardEntityDiag(Position position, int range = 1, bool ignoreWalls = false, bool sortByDistance = false)
        {
            return GetBoardEntityDiag(position, new Position(range, range), ignoreWalls, sortByDistance);
        }

        public List<BoardEntity> TilesToBoardEntities(List<Tile> tiles)
        {
            List<BoardEntity> boardEntities = new List<BoardEntity>();
            foreach (Tile tile in tiles)
            {
                if (tile.GetComponent<Tile>().BoardEntity != null)
                {
                    boardEntities.Add(tile.BoardEntity);
                }
            }
            return boardEntities;
        }

        public CharacterBoardEntity GetFirstCharacter(List<Tile> tiles)
        {
            foreach(Tile tile in tiles)
            {
                if(tile.BoardEntity != null && tile.BoardEntity is CharacterBoardEntity)
                {
                    return ((CharacterBoardEntity)tile.BoardEntity);
                }
            }
            return null;
        }

        private void generateBoard()
        {
            generator.init();
            GenerateBoardResponse response = generator.generateTiles(this, boardEntitySelector.TileSelectionManager);
            this.coordinateToTile = response.coordinateToTile;
            this.tupleToWall = response.tuppleToWall;

        }

        //public List<Tile> GetTilesDiagLineOfSight()

        public bool CheckIfBlocked(Position start, Position end)
        {
            return GetTile(start).CheckIfBlocked(GetTile(end));
        }

        public List<Tile> DFS(Position start, Position end, Team? team = null)
        {
            List<Tile> tiles = new List<Tile>();
            Tile startTile = GetTile(start);
            Tile endTile = GetTile(end);

            if (startTile == null || endTile == null)
            {
                return tiles;
            }
            Dictionary<Tile, Tile> path = new Dictionary<Tile, Tile>();

            Queue<Tile> queue = new Queue<Tile>();
            HashSet<Tile> visitedTiles = new HashSet<Tile>();
            queue.Enqueue(startTile);
            while (queue.Count != 0)
            {
                Tile currentTile = queue.Dequeue();

                List<Tile> neighbors = GetTilesNoDiag(currentTile.Position);
                neighbors.RemoveAll(
                    x => path.ContainsKey(x)
                );

                foreach (Tile nextTile in neighbors)
                {
                    if (nextTile == endTile)
                    {
                        // we found it
                        path.Add(nextTile, currentTile);

                        Tile lastTile = endTile;
                        while (lastTile != startTile)
                        {
                            tiles.Insert(0, path[lastTile]);
                            lastTile = path[lastTile];
                        }
                        return tiles;
                    }
                }
                foreach (Tile nextTile in neighbors)
                {
                    if (team == null || nextTile.BoardEntity == null || nextTile.BoardEntity.Team == team)
                    {
                        path.Add(nextTile, currentTile);
                        queue.Enqueue(nextTile);
                    }
                          
                }
            }
            return tiles;
        }

        /// <summary>
        /// another dfs, kms
        /// </summary>
        /// <param name="start"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public BoardEntity NearestBoardEntity(Position start, Team? team, BoardEntity ignore = null, bool includeStealth = false)
        {
            Queue<Tile> tiles = new Queue<Tile>();
            HashSet<Tile> visitedTiles = new HashSet<Tile>();
            tiles.Enqueue(GetTile(start));
            while(tiles.Count > 0)
            {
                Tile t = tiles.Dequeue();
                List<Tile> neighbors = GetTilesNoDiag(t.Position);
                foreach(Tile newTile in neighbors)
                {
                    if(!visitedTiles.Contains(newTile))
                    {
                        if(newTile.BoardEntity != null && ((team == null || newTile.BoardEntity.Team == team)
                            && newTile.BoardEntity != ignore) && !((CharacterBoardEntity)newTile.BoardEntity).IsStealthed())
                        {
                            return newTile.BoardEntity;                           
                        }
                        visitedTiles.Add(newTile);
                        tiles.Enqueue(newTile);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// gets all moves that can be reached with a cost of 'range'
        /// </summary>
        /// <param name="start"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Move> DFSMoves(Position start, CharacterBoardEntity character, int apLimit = 2, Team? team = null,
            HashSet<Tile> tauntTiles = null)
        {
              
            if (tauntTiles != null && tauntTiles.Count != 0)
            {
                tempCoordinateToTile = new Dictionary<Position, Tile>();
                foreach (Tile t in tauntTiles)
                {
                    tempCoordinateToTile.Add(t.Position, t);
                }
            }

            int movementStats = character.Stats.GetStatInstance().getValue(Entities.AttributeStats.StatType.Movement);
            int movementPoints = character.Stats.GetMutableStat(Entities.AttributeStats.StatType.Movement).Value;

            int range = movementPoints + movementStats * character.Stats.GetMutableStat(Entities.AttributeStats.StatType.AP).Value;

            List<Move> tiles = new List<Move>();
            Tile startTile = GetTile(start);
            HashSet<Tile> visitedTiles = new HashSet<Tile>();
            List<Move> visitingTiles = new List<Move>();
            visitingTiles.Add(new Move { destination = startTile, movementCost = 0 });

            while(visitingTiles.Count > 0)
            {
                List<Move> newVisitingTiles = new List<Move>();
                foreach (Move move in visitingTiles)
                {
                    List<Tile> neighbors = GetTilesNoDiag(move.destination.Position);

                    //cannot pass through other team, unless team is null
                    if (team != null)
                    {
                        neighbors.RemoveAll(x => (x.BoardEntity != null && x.BoardEntity.Team != team));
                    }

                    foreach (Tile processingTile in neighbors)
                    {
                        if (processingTile == startTile)
                        {
                            continue;
                        }
                        if (!visitedTiles.Contains(processingTile))
                        {
                            List<Tile> newPath = new List<Tile>(move.path);
                            newPath.Add(processingTile);
                            Move newMove = new Move
                            {
                                destination = processingTile,
                                movementCost = move.movementCost + 1,
                                path = newPath
                            };
                            int cost = newMove.movementCost - movementPoints;
                            if(cost <= 0)
                            {
                                newMove.apCost = 0;
                            }
                            else
                            {
                                newMove.apCost = Mathf.CeilToInt(((float)cost) / ((float)(movementStats)));
                            }
                            newMove.movementPointsAfterMove = movementPoints + (newMove.apCost * movementStats) - newMove.movementCost;

                            if((newMove.apCost <= apLimit) && (newMove.apCost <= character.Stats.GetMutableStat(Entities.AttributeStats.StatType.AP).Value))
                            {
                                newVisitingTiles.Add(newMove);
                                visitedTiles.Add(processingTile);
                            }

                        }
                    }
                }
                foreach (Move move in newVisitingTiles)
                {
                    tiles.Add(move);
                }
                visitingTiles = newVisitingTiles;
                
            }

            //cannot end on an already occupied tile
            tiles.RemoveAll(x => (x.destination.BoardEntity));

            tempCoordinateToTile = null;
            return tiles;
        }

        public List<Tile> GetTilesInDirection(Position start, Position direction, int distance, bool stopAtObstacle = true)
        {
            List<Tile> returnTiles = new List<Tile>();
            Position currentPosition = new Position(start);
            for(int a = 0; a < distance; a++)
            {
                currentPosition = currentPosition + direction;
                if(GetTile(currentPosition) != null)
                {
                    returnTiles.Add(GetTile(currentPosition));
                }
                else
                {
                    if(stopAtObstacle)
                    {
                        break;
                    }
                }
            }
            return returnTiles;
        }

        public List<Tile> GetTilesInAllDirections(Position start, List<Position> directions, int distance, bool stopAtObstacle = true)
        {
            List<Tile> returnTiles = new List<Tile>();
            foreach(Position p in directions)
            {
                List<Tile> currentReturnTiles = GetTilesInDirection(start, p, distance, stopAtObstacle);
                returnTiles.AddRange(currentReturnTiles);
            }
            return returnTiles;

        }

        public bool AddBoardEntity(Position position, GameObject boardEntity)
        {
            Tile tile = GetTile(position);
            if (tile != null && tile.BoardEntity == null)
            {
                boardEntity.GetComponent<BoardEntity>().Position = position;
                boardEntity.transform.position = tile.transform.position;
                tile.SetBoardEntity(boardEntity.GetComponent<BoardEntity>());
                if(UpdatedBoardEntityPosition != null)
                    UpdatedBoardEntityPosition();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveTile(Position p)
        {
            Tile t = GetTile(p);
            if (t != null && CanRemoveTile(t))
            {
                t.gameObject.SetActive(false);
                coordinateToTile.Remove(p);
                return true;
            }
            return false;
        }

        private bool CanRemoveTile(Tile t)
        {
            int beforeCount = ReachableTiles(t, new HashSet<Tile>()).Count;
            int afterCount = ReachableTiles(t, new HashSet<Tile>() {t}).Count;
            return (beforeCount - 1 == afterCount);
        }

        private HashSet<Tile> ReachableTiles(Tile start, HashSet<Tile> exclude)
        {
            HashSet<Tile> reachableTiles = new HashSet<Tile>() { start };
            List<Tile> visitingTiles = new List<Tile>() { start };
            while(visitingTiles.Count != 0)
            {
                Tile t = visitingTiles[0];
                visitingTiles.RemoveAt(0);
                List<Tile> tiles = GetTilesNoDiag(t.Position);
                foreach(Tile tile in tiles)
                {
                    if(!reachableTiles.Contains(tile) && !exclude.Contains(tile))
                    {
                        reachableTiles.Add(tile);
                        visitingTiles.Add(tile);
                    }
                }

            }
            return reachableTiles;

        }

    }
}
