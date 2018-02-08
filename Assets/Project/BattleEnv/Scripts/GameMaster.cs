using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Kas;
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
        public GameObject Jaz;
        public GameObject Bongani;
        public GameObject Lesidi;
        public GameObject Dadi;
        public GameObject Amare;
        public GameObject Tisha;

        public GameObject EnemyTank;
        public GameObject EnemyRanged;
        public GameObject EnemyBalanced;
        public GameObject EnemySpeedy;

        // Use this for initialization
        void Start()
        {
            instance = this;

            tileManager.Init(turnManager, profile);
            GameObject BE;

            MakeCharacter(Amare, new Position(0, 0));
            MakeCharacter(Tisha, new Position(0, 1));
            MakeCharacter(Lesidi, new Position(1, 0));
            MakeCharacter(Jaz, new Position(1, 1));

            MakeCharacter(EnemyRanged, new Position(5, 5));
            MakeCharacter(EnemySpeedy, new Position(6, 4));
            MakeCharacter(EnemyTank, new Position(4, 6));
            MakeCharacter(EnemyBalanced, new Position(7, 3));

            turnManager.init(boardEntitySelector, tileSelectionManager);
            turnManager.ReCalcQueue();
            turnManager.startGame();

        }

        private void MakeCharacter(GameObject character, Position position, Ka ka = null)
        {
            GameObject BE = Instantiate(character);
            BE.GetComponent<CharacterBoardEntity>().Init(position, turnManager, tileManager, boardEntitySelector, battleCalulator, ka);
        }
    }
}
