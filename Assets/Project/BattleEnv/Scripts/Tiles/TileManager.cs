using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Placeholdernamespace.Battle.Env
{
    public class TileManager : MonoBehaviour
    {

        public GameObject testBoardEntity;
        public BoardEntitySelector boardEntitySelector;
        public TileGenerator generator;

        private TurnManager turnManager;
        private Profile profile;

        private Dictionary<Tuple<Position, Position>, GameObject> tupleToWall = new Dictionary<Tuple<Position, Position>, GameObject>();
        private Dictionary<Position, Tile> coordinateToTile = new Dictionary<Position, Tile>();

        public void Init(TurnManager turnManager, Profile profile)
        {
            this.turnManager = turnManager;
            this.profile = profile;
            this.boardEntitySelector.Init();
            generateBoard();
        }

        public void MoveBoardEntity(Position p, BoardEntity entity)
        {
            entity.GetTile().BoardEntity = null;
            entity.Position = p;
            entity.GetTile().BoardEntity = entity;
            entity.transform.position = entity.GetTile().transform.position;
        }

        public Tile GetTile(Position position)
        {
            Tile returnObj;
            if (coordinateToTile.TryGetValue(position, out returnObj))
            {
                return returnObj;
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

        public List<Tile> GetAllTilesNear(Position position, Position range, bool ignoreWalls = false, bool sortByDistance = false)
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
                        if (ignoreWalls)
                        {
                            returnList.Add(neighbor);
                        }
                        else if (!startingTile.CheckIfBlocked(neighbor))
                        {
                            returnList.Add(neighbor);
                        }
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

        public List<Tile> GetAllTilesNear(Position position, int range = 1, bool ignoreWalls = false, bool sortByDistance = false)
        {
            return GetAllTilesNear(position, new Position(range, range), ignoreWalls, sortByDistance);
        }

        public List<BoardEntity> GetAllNear(Position position, Position Range, bool ignoreWalls = false, bool sortByDistance = false)
        {
            List<Tile> tiles = GetAllTilesNear(position, Range, ignoreWalls, sortByDistance);
            return tilesToBoardEntities(tiles);
        }

        public List<BoardEntity> GetAllNear(Position position, int range = 1, bool ignoreWalls = false, bool sortByDistance = false)
        {
            return GetAllNear(position, new Position(range, range), ignoreWalls, sortByDistance);
        }

        private List<BoardEntity> tilesToBoardEntities(List<Tile> tiles)
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

        private void generateBoard()
        {
            generator.init();
            GenerateBoardResponse response = generator.generateTiles(this, boardEntitySelector.TileSelectionManager);
            this.coordinateToTile = response.coordinateToTile;
            this.tupleToWall = response.tuppleToWall;

            //this should be removed later
            GameObject BE = Instantiate(testBoardEntity);
            BE.GetComponent<CharacterBoardEntity>().Init(turnManager, this, boardEntitySelector);
            AddBoardEntity(new Position(0, 0), BE);

        }

        public bool CheckIfBlocked(Position start, Position end)
        {
            return GetTile(start).CheckIfBlocked(GetTile(end));
        }

        public List<Tile> DFS(Position start, Position end)
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
            queue.Enqueue(startTile);
            while (queue.Count != 0)
            {
                Tile currentTile = queue.Dequeue();

                List<Tile> neighbors = GetAllTilesNear(currentTile.Position, 1, sortByDistance: true);
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
                        tiles.Insert(0, endTile);
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
                    path.Add(nextTile, currentTile);
                    queue.Enqueue(nextTile);
                }
            }
            return tiles;
        }

        /// <summary>
        /// gets all moves that can be reached with a cost of 'range'
        /// </summary>
        /// <param name="start"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Move> DFSMoves(Position start, int range)
        {
            List<Move> tiles = new List<Move>();
            Tile startTile = GetTile(start);
            HashSet<Tile> visitedTiles = new HashSet<Tile>();
            List<Move> visitingTiles = new List<Move>();
            visitingTiles.Add(new Move { destination = startTile, movementCost = 0 });

            for (int step = 1; step <= range; step++)
            {
                List<Move> newVisitingTiles = new List<Move>();
                foreach (Move move in visitingTiles)
                {
                    List<Tile> neighbors = GetAllTilesNear(move.destination.Position, 1, sortByDistance: true);
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
                            newVisitingTiles.Add(new Move
                            {
                                destination = processingTile,
                                movementCost = move.movementCost + 1,
                                path = newPath
                            });
                            visitedTiles.Add(processingTile);

                        }
                    }
                }
                foreach (Move move in newVisitingTiles)
                {
                    tiles.Add(move);
                }
                visitingTiles = newVisitingTiles;
            }
            return tiles;
        }

        public bool AddBoardEntity(Position position, GameObject boardEntity)
        {
            Tile tile = GetTile(position);
            if (tile != null && tile.BoardEntity == null)
            {
                boardEntity.GetComponent<BoardEntity>().Position = position;
                boardEntity.transform.position = tile.transform.position;
                tile.SetBoardEntity(boardEntity.GetComponent<BoardEntity>());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
