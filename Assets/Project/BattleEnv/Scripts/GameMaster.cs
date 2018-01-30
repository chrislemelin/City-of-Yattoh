using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle
{
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
        public GameObject Player1;
        public GameObject Player2;
        public GameObject Enemy1;

        // Use this for initialization
        void Start()
        {
            instance = this;

            tileManager.Init(turnManager, profile);

            GameObject BE = Instantiate(Player1);
            BE.GetComponent<CharacterBoardEntity>().Init(new Position(1, 2), turnManager, tileManager, boardEntitySelector, battleCalulator);

            
            BE = Instantiate(Player2);
            BE.GetComponent<CharacterBoardEntity>().Init(new Position(1, 5), turnManager, tileManager, boardEntitySelector, battleCalulator);
            

            BE = Instantiate(Enemy1);
            BE.GetComponent<CharacterBoardEntity>().Init(new Position(1, 0), turnManager, tileManager, boardEntitySelector, battleCalulator);
            
            turnManager.init(boardEntitySelector, tileSelectionManager);
            turnManager.ReCalcQueue();
            turnManager.startGame();

        }
    }
}
