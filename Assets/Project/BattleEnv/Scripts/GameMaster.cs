using Placeholdernamespace.Battle;
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
        public TileSelectionManager pathSelectManager;
        public TileManager tileManager;
        public Profile profile;

        // Use this for initialization
        void Start()
        {
            instance = this;

            pathSelectManager.Init(turnManager, profile);
            tileManager.Init(pathSelectManager, turnManager, profile);


        }
    }
}
