using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour {

    protected TileManager tileManager;
    protected List<Action> actionQueue;
    protected CharacterBoardEntity characterBoardEntity;
    protected Skill skill;

    public void Init(TileManager tileManager, CharacterBoardEntity characterBoardEntity)
    {
        this.tileManager = tileManager;
        this.characterBoardEntity = characterBoardEntity;
    }

  
}
