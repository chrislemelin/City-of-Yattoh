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
        private GameObject healthBar;

        [SerializeField]
        protected Team team;
        public Team Team
        {
            get { return team; }
        }

        protected List<Skill> skills = new List<Skill>();
        public List<Skill> Skills
        {
            get { return skills; }
        }

        protected BoardEntitySelector boardEntitySelector;

        protected TurnManager turnManager;
        protected TileManager tileManager;
        protected TileSelectionManager tileSelectionManager;

        public Tile GetTile()
        {
            return tileManager.GetTile(position);
        }

        protected Position position;
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
        }

        [SerializeField]
        protected new string name = "";
        public string Name
        {
            get { return name; }
        }

        public virtual void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            healthBar = Instantiate(healthBar);
            healthBar.transform.SetParent(FindObjectOfType<Canvas>().gameObject.transform);
            healthBar.GetComponent<UIFollow>().target = gameObject;
            healthBar.transform.SetAsFirstSibling();
            healthBar.transform.position = new Vector3(100000, 100000);

            this.turnManager = turnManager;
            this.tileManager = tileManager;
            this.boardEntitySelector = boardEntitySelector;

            tileManager.AddBoardEntity(startingPosition, gameObject);

            stats.updateStatHandler += UpdateUi;
            stats.Start(this);

            turnManager.AddBoardEntity(this);
            UpdateUi();
        }

        public abstract void StartMyTurn();

        public virtual void OnSelect()
        {
            boardEntitySelector.setSelectedBoardEntity(this);
        }

        public void UpdateUi()
        {
            if (updateStatHandler != null)
            {
                updateStatHandler(this);
            }
            if (healthBar != null)
            {
                float newHealth = (float)stats.GetMutableStat(StatType.Health).Value / (float)stats.GetStatInstance().getValue(StatType.Health);
                healthBar.GetComponent<UIBar>().SetValue(newHealth);
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


    }

    public enum Team { Player, Enemy, Neutral, Environment }
}