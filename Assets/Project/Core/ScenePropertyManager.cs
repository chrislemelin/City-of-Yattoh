using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePropertyManager {

    private static ScenePropertyManager instance;
    public static ScenePropertyManager Instance
    {
        get{
            if(instance == null)
            {
                instance = new ScenePropertyManager();
            }
            return instance;
        }
    }
    
   // public Dictionary<Position, CharacterType> characters;
    public List<CharacterType> characters = new List<CharacterType>() {CharacterType.PlayerBongani, CharacterType.PlayerTisha,
        CharacterType.PlayerJaz, CharacterType.PlayerDadi};

    public List<Tuple<CharacterType, Ka>> characters2 = new List<Tuple<CharacterType, Ka>>() {
        new Tuple<CharacterType, Ka>(CharacterType.PlayerBongani, null),
        new Tuple<CharacterType, Ka>(CharacterType.PlayerAmare, null),
        new Tuple<CharacterType, Ka>(CharacterType.PlayerJaz, null),
        new Tuple<CharacterType, Ka>(CharacterType.PlayerTisha, null)
    };



}
