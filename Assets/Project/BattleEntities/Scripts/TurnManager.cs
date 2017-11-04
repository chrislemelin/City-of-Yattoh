﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TurnManager : MonoBehaviour {

    public GUIText display;

    public delegate void NewTurnHandler(object sender, EventArgs e);
    public event NewTurnHandler OnNewTurn;

    public static TurnManager instance;
    public static TurnManager Instance
    {
        get { return instance; } 
    }

    private BoardEntity currentBoardEntity;
    public BoardEntity CurrentBoardEntity
    {
        get { return currentBoardEntity; }
    }

    private List<BoardEntity> enities = new List<BoardEntity>();
    private List<BoardEntity> turnQueue = new List<BoardEntity>();
    private int queueLength = 5;

    private void Awake()
    {
        instance = this;
    }

    public void AddBoardEntity(BoardEntity boardEntity)
    {
        enities.Add(boardEntity);
        ReCalcQueue();
        NextTurn();
    }

    public void RemoveBoardEntity(BoardEntity boardEntity)
    {
        enities.Remove(boardEntity);
        ReCalcQueue();
    }

    public void NextTurn()
    {
        if (turnQueue.Count == 0)
        {
            ReCalcQueue();
        }
        currentBoardEntity = turnQueue[0];
        turnQueue.RemoveAt(0);
        UpdateGui();
        NewTurn();
    }

    public void ClearBoardEnities()
    {
        enities.Clear();
        ReCalcQueue();
    }

    public void NewTurn()
    {
        if (OnNewTurn != null)
            OnNewTurn(this, new EventArgs());
    }

    private void UpdateGui()
    {
        string newText = "";
        foreach(BoardEntity entity in turnQueue)
        {
            newText += entity.name + '\n';
        }
        if(display != null)
        {
            display.text = newText;
        }
    }

    private void ReCalcQueue()
    {
        turnQueue.Clear();
        turnQueue.AddRange(enities);
        turnQueue.OrderBy(x => x.stats.GetStatInstance().GetStat(StatType.Speed));
        UpdateGui();
    }
}
