using Placeholdernamespace.Battle;
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

        public Dictionary<CharacterType, GameObject> boardEntityCharacters = new Dictionary<CharacterType, GameObject>();

        public List<GameObject> boardEntities;
      
        // Use this for initialization
        void Start()
        {
            instance = this;

            MakeDictionary();

            tileManager.Init(turnManager, profile);
            GameObject BE;

            Position currentPosition = new Position(0, 0);
            foreach(Tuple<CharacterType,Ka> character in ScenePropertyManager.Instance.characters2)
            {
                MakeCharacter(character.first, currentPosition, character.second);
                currentPosition = currentPosition + new Position(0, 1);
            }

            /*
            MakeCharacter(ScenePropertyManager.Instance.characters[0], new Position(1, 1), CharacterType.PlayerAmare);
            MakeCharacter(ScenePropertyManager.Instance.characters[1], new Position(0, 1));
            MakeCharacter(ScenePropertyManager.Instance.characters[2], new Position(1, 0));
            MakeCharacter(ScenePropertyManager.Instance.characters[3], new Position(0, 0));
            */

            MakeCharacter(CharacterType.EnemyRanged, new Position(5, 5));
            MakeCharacter(CharacterType.EnemySpeedy, new Position(6, 4));
            MakeCharacter(CharacterType.EnemyTank, new Position(4, 6));
            MakeCharacter(CharacterType.EnemyBalanced, new Position(7, 3));
            
            turnManager.init(boardEntitySelector, tileSelectionManager);
            turnManager.ReCalcQueue();
            turnManager.startGame();

        }

        private void MakeDictionary()
        {
            foreach(GameObject character in boardEntities)
            {
                if(character.GetComponent<CharacterBoardEntity>() != null)
                    boardEntityCharacters.Add(character.GetComponent<CharacterBoardEntity>().CharcaterType, character);               
            }
        }

        private void MakeCharacter(CharacterType characterType, Position position, Ka ka = null)
        {
            GameObject character = boardEntityCharacters[characterType];
           
            GameObject BE = Instantiate(character);
            BE.GetComponent<CharacterBoardEntity>().Init(position, turnManager, tileManager, boardEntitySelector, battleCalulator, ka);
        }
    }
}
