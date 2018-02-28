﻿using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Instances;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Placeholdernamespace.Battle
{
    public enum CharacterType
    {
        PlayerJaz, PlayerBongani, PlayerLesidi, PlayerAmare, PlayerTisha, PlayerDadi,
        EnemyTank, EnemyBalanced, EnemyRanged, EnemySpeedy
    }

    public class GameMaster : MonoBehaviour
    {

        private static GameMaster instance;
        public static GameMaster Instance
        {
            get { return instance; }
        }

        public TurnManager turnManager;
        public TileManager tileManager;
        public Profile profile;

        public BattleCalculator battleCalulator;
        public BoardEntitySelector boardEntitySelector;
        public TileSelectionManager tileSelectionManager;

        public Dictionary<CharacterType, CharacterBoardEntity> boardEntityCharacters = new Dictionary<CharacterType, CharacterBoardEntity>();

        public List<GameObject> boardEntities;
      
        // Use this for initialization
        void Start()
        {
            instance = this;

            MakeDictionary();

            tileManager.Init(turnManager, profile);

            Position currentPosition = new Position(0, 0);
            foreach(Tuple<CharacterBoardEntity, Ka> character in ScenePropertyManager.Instance.GetCharacterParty())
            {
                MakeCharacter(character.first, currentPosition, character.second);
                currentPosition = currentPosition + new Position(0, 1);
            }
            SpawnEnemies();

            turnManager.init(boardEntitySelector, tileSelectionManager);
            turnManager.ReCalcQueue();
            turnManager.startGame();

        }

        private void MakeDictionary()
        {
            foreach(GameObject character in boardEntities)
            {
                if(character.GetComponent<CharacterBoardEntity>() != null)
                    boardEntityCharacters.Add(character.GetComponent<CharacterBoardEntity>().CharcaterType, character.GetComponent<CharacterBoardEntity>());               
            }
        }

        private void MakeCharacter(CharacterBoardEntity character, Position position, Ka ka = null)
        {
            GameObject characterObj = character.gameObject;
           
            GameObject BE = Instantiate(characterObj);
            BE.GetComponent<CharacterBoardEntity>().Init(position, turnManager, tileManager, boardEntitySelector, battleCalulator, ka);
        }

        private CharacterBoardEntity GetCharacterBoardEntity(CharacterType type)
        {
            foreach(CharacterBoardEntity compareType in boardEntityCharacters.Values)
            {
                if(compareType.CharcaterType == type)
                {
                    return compareType;
                }
            }
            return null;

        }

        private void SpawnEnemies()
        {
            foreach(KeyValuePair<Position, CharacterType> value in ScenePropertyManager.Instance.Enemies)
            {
                MakeCharacter(GetCharacterBoardEntity(value.Value), value.Key);
            }
        }
    }
}
