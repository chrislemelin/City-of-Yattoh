using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private Position position;
    public Position Position
    {
        get { return position; }
    }

    private BoardEntity boardEntity;
    public BoardEntity BoardEntity
    {
        get { return boardEntity; }
        set { boardEntity = value; }
    }

    public void init(Position position)
    {
        this.position = position;
    }

    public void SetBoardEntity(BoardEntity boardEntity)
    {
        this.boardEntity = boardEntity; 
    }

    public bool CheckIfBlocked(Tile tile)
    {
        Vector2 currentPos = transform.position;
        Vector2 tilePos = tile.transform.position;
        Vector2 rayDirection = tilePos - currentPos;

        RaycastHit2D hit = Physics2D.Raycast(currentPos, rayDirection);
        if ((Vector2)hit.transform.position == tilePos)
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
        return TileManager.Instance.GetTile(Position + offset).GetComponent<Tile>();       
    }

    public BoardEntity GetInReferencTo(Position offset)
    {

        return TileManager.Instance.GetTile(Position + offset).GetComponent<Tile>().boardEntity;
        
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
        return TileManager.Instance.GetAllTilesNear(Position, range, ignoreWalls);
    }

    public List<Tile> GetAllTilesNear(int range = 1, bool ignoreWalls = false)
    {
        return TileManager.Instance.GetAllTilesNear(Position, range, ignoreWalls);
    }

    public List<BoardEntity> GetAllNear(Position range, bool ignoreWalls = false)
    {
        return TileManager.Instance.GetAllNear(Position, range, ignoreWalls);
    }
 
    public List<BoardEntity> GetAllNear(int range = 1, bool ignoreWalls = false)
    {
        return TileManager.Instance.GetAllNear(Position, range, ignoreWalls);
    }

}
