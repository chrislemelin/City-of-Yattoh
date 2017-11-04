using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBoardEntity: BoardEntity{

    public float speed;

    private Tile target = null;
    private List<Tile> path;

    public override void Start()
    {
        base.Start();
    }

    public override List<Move> MoveSet()
    {
        return TileManager.Instance.DFSMoves(tile.Position, stats.MovementPoints);
    }

    public List<Move> TheoritcalMoveSet(Position position, int movementPoints)
    {
        return TileManager.Instance.DFSMoves(tile.Position, movementPoints);
    }

    public void ExecuteMove(Move move)
    {
        this.stats.ModifyMovementPoint(move.movementCost);
        PathSelectManager.Instance.pause = true;
        path = move.path;
        if(path.Count > 0)
        {
            target = path[0];
            path.Remove(target);
        }
    }

    private void checkAtTarget()
    {
        if(transform.position == target.transform.position)
        {
            TileManager.Instance.MoveBoardEntity(target.Position, this);

            if(path.Count == 0)
            {
                target = null;
                PathSelectManager.Instance.pause = false;
                PathSelectManager.Instance.ClearPath();

            }
            else
            {
                target = path[0];
                path.Remove(target);
            }
        }
    }

    private void doMovement()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    void Update()
    {
        if(target != null)
        {
            doMovement();
            checkAtTarget();
        }

    }

    public override void NewTurnHandler(object sender, EventArgs args)
    {
        stats.NewTurn();
    }
}
