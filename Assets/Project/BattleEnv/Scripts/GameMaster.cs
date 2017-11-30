using Placeholdernamespace.Battle;
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

        public BoardEntitySelector boardEntitySelector;
        public GameObject Player1;
        public GameObject Enemy1;

        // Use this for initialization
        void Start()
        {
            instance = this;

            tileManager.Init(turnManager, profile);

            GameObject BE = Instantiate(Player1);
            BE.GetComponent<CharacterBoardEntity>().Init(turnManager, tileManager, boardEntitySelector);
            tileManager.AddBoardEntity(new Position(0, 0), BE);

            BE = Instantiate(Enemy1);
            BE.GetComponent<CharacterBoardEntity>().Init(turnManager, tileManager, boardEntitySelector);
            tileManager.AddBoardEntity(new Position(1, 1), BE);

        }
    }
}
