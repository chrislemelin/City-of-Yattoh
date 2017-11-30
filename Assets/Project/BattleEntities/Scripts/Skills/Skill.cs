using Placeholdernamespace.Battle.Calculator;
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
        protected BattleCalculator battleCalculator; 

        public string Title
        {
            get { return title; }
        }

        public void Init(TileManager tileManager, BoardEntity boardEntity, BattleCalculator battleCalculator)
        {
            this.tileManager = tileManager;
            this.boardEntity = boardEntity;
            this.battleCalculator = battleCalculator;
        }

        public abstract List<Tile> TileSet();

        public abstract void Action(Tile t);

        public virtual bool IsActive()
        {
            return TileSet().Count > 0;
        }

        protected Dictionary<SkillModifierType, int> getSkillModifiers(Dictionary<SkillModifierType, int> dict)
        {
            Dictionary<SkillModifierType, float> floatDict = new Dictionary<SkillModifierType, float>();
            List<SkillModifier> modifiers = new List<SkillModifier>();
            foreach(SkillModifierType type in dict.Keys)
            {
                floatDict[type] = dict[type];
                foreach (SkillModifier mod in modifiers)
                {
                    if(mod.Application == SkillModifierApplication.Add && mod.Type == type)
                    {
                        floatDict[type] = mod.Apply(floatDict[type], type);
                    }

                    if (mod.Application == SkillModifierApplication.Mult && mod.Type == type)
                    {
                        floatDict[type] = mod.Apply(floatDict[type], type);
                    }

                    if (mod.Application == SkillModifierApplication.AddNoMult && mod.Type == type)
                    {
                        floatDict[type] = mod.Apply(floatDict[type], type);
                    }
                }
            }
            Dictionary<SkillModifierType, int> intDict = new Dictionary<SkillModifierType, int>();
            foreach(SkillModifierType type in floatDict.Keys)
            {
                intDict[type] = (int)floatDict[type];
            }

            return intDict;
        }

        protected List<Tile> TeamTiles(List<Tile> tiles, Team? team)
        {
            tiles.RemoveAll(t => t.BoardEntity == null || team == null && t.BoardEntity.Team != team);
            return tiles;
        }
    }

  
}
