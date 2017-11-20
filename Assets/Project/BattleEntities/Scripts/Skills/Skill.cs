using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Skills
{
    public abstract class Skill
    {
        protected string title;
        protected TileManager tileManager;
        protected BoardEntity boardEntity;

        public string Title
        {
            get { return title; }
        }

        public void Init(TileManager tileManager, BoardEntity boardEntity)
        {
            this.tileManager = tileManager;
            this.boardEntity = boardEntity;
        }

        public abstract List<Tile> TileSet();

        public abstract void Action(Tile t);

        public virtual bool IsActive()
        {
            return TileSet().Count > 0;
        }
    }
}
