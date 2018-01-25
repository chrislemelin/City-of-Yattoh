using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManagerMarker : MonoBehaviour {

    [SerializeField]
    TileManager tileManager;

    private void Start()
    {
        //tileManager.UpdatedBoardEntityPosition += Reorder;
    }

    private void Reorder()
    {
        /*
        List<CharacterBoardEntity> chars = CharacterBoardEntity.AllCharacterBoardEntities;
        chars = chars.OrderBy( (x) =>x.Position.x).ToList();
        foreach(CharacterBoardEntity ent in chars)
            */
        
    }

}
