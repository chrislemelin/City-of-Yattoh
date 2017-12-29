using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Skills
{
    public abstract class Skill
    {
        protected string title;
        protected TileManager tileManager;
        protected CharacterBoardEntity boardEntity;
        protected BattleCalculator battleCalculator;

        protected int APCost;

        public string Title
        {
            get { return title; }
        }

        public void Init(TileManager tileManager, CharacterBoardEntity boardEntity, BattleCalculator battleCalculator)
        {
            this.tileManager = tileManager;
            this.boardEntity = boardEntity;
            this.battleCalculator = battleCalculator;
        }

        public List<Tile> TileSet()
        {
            return TileSetHelper(boardEntity.GetTile().Position);
        }

        public List<Tile> TheoreticalTileSet(Position p)
        {
            return TileSetHelper(p);
        }

        protected abstract List<Tile> TileSetHelper(Position p);

        public abstract void Action(Tile t, Action callback = null);

        public virtual bool IsActive()
        {
            return TileSet().Count > 0 && CanAffortAPCost();
        }

        public SkillReport TheoreticalAction(Tile t)
        {
            DamagePackageInternal damagePackage = GenerateDamagePackage();
            DamagePackage damage = new DamagePackage(damagePackage);
            SkillReport report = battleCalculator.ExecuteSkillHelper(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, damage);
            return report;
        }

        protected Dictionary<SkillModifierType, int> getSkillModifiers(Dictionary<SkillModifierType, int> dict)
        {
            Dictionary<SkillModifierType, float> floatDict = new Dictionary<SkillModifierType, float>();
            List<SkillModifier> modifiers = new List<SkillModifier>();

            //modifiers.Add(new SkillModifier(SkillModifierType.Power, SkillModifierApplication.Add, 2));

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
            tiles.RemoveAll(t => t.BoardEntity == null || team != null && t.BoardEntity.Team == team);
            return tiles;
        }

        protected bool CanAffortAPCost()
        {
            return (boardEntity.Stats.GetMutableStat(AttributeStats.StatType.AP).Value >= GetAPCost());
        }

        public int GetAPCost()
        {
            return GetSkillModifier(SkillModifierType.EnergyCost, APCost);
        }

        protected int GetSkillModifier(SkillModifierType type, int initialValue)
        {
            Dictionary<SkillModifierType, int> dict = new Dictionary<SkillModifierType, int>();
            dict[type] = initialValue;
            Dictionary<SkillModifierType, int> effectiveStats = getSkillModifiers(dict);
            return effectiveStats[type];
        }

        /// <summary>
        /// override this to generate damage package
        /// </summary>
        /// <returns></returns>
        protected DamagePackageInternal GenerateDamagePackage()
        {
            int basePower = boardEntity.Stats.GetStatInstance().getValue(AttributeStats.StatType.Strength);
            Dictionary<SkillModifierType, int> baseStats = new Dictionary<SkillModifierType, int>();
            baseStats[SkillModifierType.Power] = basePower;
            Dictionary<SkillModifierType, int> effectiveStats = getSkillModifiers(baseStats);

            int effectivePower = effectiveStats[SkillModifierType.Power];
            return new DamagePackageInternal(effectivePower, DamageType.physical);
        }


    }

    public enum DamageType { physical, pure };

    public class DamagePackageInternal
    {
        public float damage;
        public float Damage
        {
            get { return damage; }
        }

        public DamageType type;
        public DamageType Type
        {
            get { return type; }
        }

        public DamagePackageInternal(float damage, DamageType type)
        {
            this.damage = damage;
            this.type = type;
        }
    }


}
