using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Placeholdernamespace.Common.Interfaces;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.UI;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Calculator;
using UnityEngine.UI;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Kas;

namespace Placeholdernamespace.Battle.Entities
{
    public abstract class BoardEntity : MonoBehaviour, ISelectable
    {
        public delegate void UpdateState(object sender);
        public event UpdateState updateStatHandler;

        [SerializeField]
        private Sprite profileImage;
        public Sprite ProfileImage
        {
            get { return profileImage; }

        }
        [SerializeField]
        private Sprite profileImageCircle;
        public Sprite ProfileImageCircle
        {
            get { return profileImageCircle; }
        }

        [SerializeField]
        protected GameObject healthBar;
        protected GameObject healthBarInstance;

        [SerializeField]
        protected Team team;
        public Team Team
        {
            get { return team; }
        }  

        protected BoardEntitySelector boardEntitySelector;
        bool isInit = false;


        protected TurnManager turnManager;
        protected TileManager tileManager;
        protected TileSelectionManager tileSelectionManager;

        public Tile GetTile()
        {
            return tileManager.GetTile(Position);
        }

        protected Position position = new Position(0, 0);
        public Position Position
        {
            get { return position; }
            set {
                if (value != null)
                {
                    position = value;
                }
                else
                {
                    position = value;
                }
            }
        }


        public virtual List<Move> MoveSet()
        {
            // cant move by default
            return new List<Move>();
        }

        [SerializeField]
        protected Stats stats;
        public Stats Stats
        {
            get { return stats; }
            set { stats = value; }
        }

        [SerializeField]
        protected new string name = "";
        public string Name
        {
            get { return name; }
        }

        protected BattleCalculator  battleCalculator;

        public virtual void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, 
            BattleCalculator battleCalculator, Ka ka = null)
        {
            healthBarInstance = Instantiate(healthBar);
            healthBarInstance.transform.SetParent(FindObjectOfType<HealthBarContainer>().gameObject.transform);
            healthBarInstance.GetComponent<UIFollow>().target = gameObject;
            healthBarInstance.transform.SetAsFirstSibling();
            healthBarInstance.transform.position = new Vector3(100000, 100000);

            this.turnManager = turnManager;
            this.tileManager = tileManager;
            this.boardEntitySelector = boardEntitySelector;
            this.battleCalculator = battleCalculator;

            isInit = true;
            tileManager.AddBoardEntity(startingPosition, gameObject);
            position = startingPosition;

            stats.updateStatHandler += UpdateUi;
            stats.Start(this);

            turnManager.AddBoardEntity((CharacterBoardEntity)this);
            UpdateUi();
        }

        public abstract void StartMyTurn();

        public virtual void OnSelect()
        {
            boardEntitySelector.setSelectedBoardEntity(this);
        }

        public void UpdateUi()
        {
            if (healthBarInstance != null)
            {
                float newHealth = (float)Stats.GetMutableStat(StatType.Health).Value / (float)Stats.GetStatInstance().getValue(StatType.Health);
                healthBarInstance.GetComponent<UIBar>().SetValue(newHealth);
            }
        }


        public void Hover()
        {
            boardEntitySelector.Hover(this);
        }

        public void ExitHover()
        {
            boardEntitySelector.ExitHover();
        }

        public abstract void AddPassive(Passive passive);
        public abstract void AddSkill(Skill skill); 

    }

    public enum Team { Player, Enemy, Neutral, Environment, Null }
}