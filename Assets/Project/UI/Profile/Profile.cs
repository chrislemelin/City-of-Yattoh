﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour {

    private static Profile instance;
    public static Profile Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject titleGameObject;
    [SerializeField]
    private GameObject textGameObject;
    [SerializeField]
    private GameObject panel;

    private List<GameObject> texts = new List<GameObject>();
    private BoardEntity currentBoardEntity;

    public void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void UpdateProfile(BoardEntity boardEntity)
    {
        if(currentBoardEntity != null)
        {
            currentBoardEntity.Stats.updateStatHandler -= RefreshProfile;
        }
        currentBoardEntity = boardEntity;
        if (boardEntity == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            currentBoardEntity.Stats.updateStatHandler += RefreshProfile;
            gameObject.SetActive(true);
            processBoardEntity(boardEntity);
        }

    }

    private void processBoardEntity(BoardEntity boardEntity)
    {
        foreach(GameObject g in texts)
        {
            Destroy(g);   
        }

        AddTitle(boardEntity.Name);
        AddText("Movement Points: " + boardEntity.Stats.MovementPoints + "/"+ boardEntity.stats.BaseStats.getValue(StatType.Movement));
        EvaluateStats(boardEntity);

    }

    private void RefreshProfile(object sender)
    {
        processBoardEntity(currentBoardEntity);
    }

    private void EvaluateStats(BoardEntity boardEntity)
    {
        foreach(Stat stat in boardEntity.stats.GetStatInstance().GetStats())
        {
            EvaluateStat(stat);
        }
    }

    private void EvaluateStat(Stat stat)
    {
        AddText(stat.Name + ":" + stat.Value);
    }

    private void AddTitle(string text)
    {
        GameObject titleName = Instantiate(titleGameObject);
        titleName.GetComponent<Text>().text = text;
        titleName.transform.parent = panel.transform;
        texts.Add(titleName);
    }

    private void AddText(string text)
    {
        GameObject movementStat = Instantiate(textGameObject);
        movementStat.GetComponent<Text>().text = text;
        movementStat.transform.parent = panel.transform;
        texts.Add(movementStat);
    }

}
