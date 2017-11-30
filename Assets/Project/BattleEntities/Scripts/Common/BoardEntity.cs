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

namespace Placeholdernamespace.Battle.Entities
{
    public abstract class BoardEntity : MonoBehaviour, ISelectable
    {
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
        public Stats stats;
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

        public virtual void Init(TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector)
        {
            this.turnManager = turnManager;
            this.tileManager = tileManager;
            this.boardEntitySelector = boardEntitySelector;
            stats.Start();

            Skill basicAttack = new BasicAttack();
            basicAttack.Init(tileManager, this);
            skills.Add(basicAttack);

            turnManager.AddBoardEntity(this);
        }

        public abstract void StartMyTurn();

        public virtual void OnSelect()
        {
            boardEntitySelector.setSelectedBoardEntity(this);
        }
    }
}