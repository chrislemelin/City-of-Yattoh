using Placeholdernamespace.Battle.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Env
{
    [System.Serializable]
    public class TileGenerator : MonoBehaviour
    {

        public GameObject Board;
        public float spacing;
        // treating the field it like a square that is rotated 45 counter clockwise
        public int height;
        public int width;

        [TextArea(3, 10)]
        public string map;

        public GameObject tile;
        public GameObject wall;
        public GameObject wallH;

        private Vector3 up;
        private Vector3 right;

        private char WALL_H = '|';
        private char WALL_V = '-';
        //private char WALL_V_SEPERATOR = '|';
        private char[] NEWLINE = { '\0', '\n' };
        private char TILE_NORMAL = 'o';
        private char WALL = 'x';

        public void init()
        {
            Renderer renderer = tile.GetComponentInChildren<Renderer>();
            up = new Vector3(-renderer.bounds.size.x / 2 - spacing, renderer.bounds.size.y / 2 + spacing, 0);
            right = new Vector3(renderer.bounds.size.x / 2 + spacing, renderer.bounds.size.y / 2 + spacing, 0);
        }


        public GenerateBoardResponse generateTiles(TileManager tileManager, TileSelectionManager pathSelectManager)
        {
            GenerateBoardResponse response = new GenerateBoardResponse();
            string[] mapItems = map.Split(NEWLINE);

            int y = 0;
            foreach (string line in mapItems)
            {
                bool horizontal = line.Contains(WALL_V + "");
                char[] chars = line.ToCharArray();
                for (int index = 0; index < chars.Length; index++)
                {
                    char c = chars[index];
                    int x = index ;
                    if (c == TILE_NORMAL)
                    {
                        Position newPosition = new Position(x, y);
                        GameObject newTile = Instantiate(tile);
                        newTile.transform.position = transform.position + (x * right) + (y * up);
                        newTile.GetComponent<Tile>().Init(newPosition, tileManager);
                        newTile.GetComponentInChildren<OutlineOnHover>().Init(pathSelectManager);
                        newTile.GetComponentInChildren<PathOnClick>().Init(pathSelectManager);
                        response.coordinateToTile.Add(newPosition, newTile.GetComponent<Tile>());

                        newTile.transform.SetParent(Board.transform);
                    }
                    if( c == WALL)
                    {
                        Position newPosition = new Position(x, y);
                        GameObject newTile = Instantiate(wall);
                        newTile.transform.position = transform.position + (x * right) + (y * up);
                        //newTile.GetComponent<Tile>().Init(newPosition, tileManager);
                        //newTile.GetComponentInChildren<OutlineOnHover>().Init(pathSelectManager);
                        //newTile.GetComponentInChildren<PathOnClick>().Init(pathSelectManager);
                        //response.coordinateToTile.Add(newPosition, newTile.GetComponent<Tile>());

                        newTile.transform.SetParent(Board.transform);
                    }
                    if (c == WALL_H)
                    {
                        Position pos1 = new Position(x, y);
                        Position pos2 = new Position(x + 1, y);
                        GameObject newWall = Instantiate(wallH);
                        newWall.transform.position = transform.position + (x * right) + (y * up) + (.5f * right);
                        response.tuppleToWall.Add(new Tuple<Position, Position>(pos1, pos2), newWall);

                        newWall.transform.parent = Board.transform;
                    }
                    if (c == WALL_V)
                    {
                        Position pos1 = new Position(x, y);
                        Position pos2 = new Position(x, y + 1);
                        GameObject newWall = Instantiate(wallH);
                        newWall.transform.position = transform.position + (x * right) + (y * up) + (.5f * up);
                        newWall.transform.Rotate(new Vector3(0, 0, 1), 90);
                        response.tuppleToWall.Add(new Tuple<Position, Position>(pos1, pos2), newWall);

                        newWall.transform.SetParent(Board.transform);
                    }
                }
                if (!horizontal)
                {
                    y++;
                }
            }
            return response;
        }

    }

    public class GenerateBoardResponse
    {
        public Dictionary<Position, Tile> coordinateToTile = new Dictionary<Position, Tile>();
        public Dictionary<Tuple<Position, Position>, GameObject> tuppleToWall = new Dictionary<Tuple<Position, Position>, GameObject>();
    }
}