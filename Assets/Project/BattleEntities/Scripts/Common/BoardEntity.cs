using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BoardEntity: MonoBehaviour {

    public virtual void Start()
    {
        stats.Start();
        TurnManager.Instance.OnNewTurn += new TurnManager.NewTurnHandler(NewTurnHandler);
        TurnManager.Instance.AddBoardEntity(this);
    }

    protected Tile tile;
    public Tile Tile
    {
        get { return tile; }
        set { tile = value; }
    }

    public virtual List<Move> MoveSet()
    {
        // cant move by default
        return new List<Move>();
    }

    [SerializeField]
    public Stats stats;
    public Stats Stats
    {
        get { return stats; }
    }

    [SerializeField]
    protected string name = "";
    public string Name
    {
        get { return name; }
    }

    public abstract void NewTurnHandler(object sender, EventArgs args);


}
