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

namespace Placeholdernamespace.Battle.Entities
{
    public abstract class BoardEntity : MonoBehaviour, ISelectable
    {

        protected TurnManager turnManager;
        protected TileManager tileManager;
        protected TileSelectionManager tileSelectionManager;
        protected Profile profile;

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
        public AttributeStats.Stats stats;
        public AttributeStats.Stats Stats
        {
            get { return stats; }
        }

        [SerializeField]
        protected string name = "";
        public string Name
        {
            get { return name; }
        }

        public virtual void Init(TurnManager turnManager, TileManager tileManager, TileSelectionManager tileSelectionManager, Profile profile)
        {
            this.turnManager = turnManager;
            this.tileManager = tileManager;
            this.tileSelectionManager = tileSelectionManager;
            this.profile = profile;
            stats.Start();
            turnManager.AddBoardEntity(this);
        }

        public abstract void MyTurn();

        public virtual void OnSelect()
        {
            profile.UpdateProfile(this);
        }
    }
}