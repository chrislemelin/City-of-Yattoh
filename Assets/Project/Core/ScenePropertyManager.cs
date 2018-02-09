using Placeholdernamespace.Battle;
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



}
