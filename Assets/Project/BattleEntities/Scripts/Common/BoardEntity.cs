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

namespace Placeholdernamespace.Battle.Entities
{
    public abstract class BoardEntity : MonoBehaviour, ISelectable
    {
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
            set { position = value; }
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
        protected string name = "";
        public string Name
        {
            get { return name; }
        }

        public virtual void Init(TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            this.turnManager = turnManager;
            this.tileManager = tileManager;
            this.boardEntitySelector = boardEntitySelector;
            stats.Start();



            turnManager.AddBoardEntity(this);
        }

        public abstract void StartMyTurn();

        public virtual void OnSelect()
        {
            boardEntitySelector.setSelectedBoardEntity(this);
        }
    }

    public enum Team { Player, Enemy, Neutral, Environment }
}