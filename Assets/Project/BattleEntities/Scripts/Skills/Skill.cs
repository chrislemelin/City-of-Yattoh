using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.Skills
{
    public abstract class Skill
    {
        [SerializeField]
        protected Color? selectColor = Color.cyan;
        [SerializeField]
        protected Color? highlightColor = Color.blue;

        [SerializeField]
        protected static Profile profile;

        protected string title;
        protected TileManager tileManager;
        protected CharacterBoardEntity boardEntity;
        protected BattleCalculator battleCalculator;

        [SerializeField]
        protected string description;

        protected int? apCost = null;
        public int GetAPCost()
        {
            int? value = getSkillModifiers(SkillModifierType.APCost, GetAPCostInternal());
            if(value == null)
            {
                return 0;
            }
            return (int)value;
        }
        protected virtual int? GetAPCostInternal()
        {
            return apCost;
        }

        protected int? strength = null;
        public int GetStrength()
        {
            int? value = getSkillModifiers(SkillModifierType.Power, GetStrengthInternal());
            if (value == null)
            {
                return 0;
            }
            return (int)value;
        }
        protected virtual int? GetStrengthInternal()
        {
            return strength;
        }

        protected int? coolDown = null;
        public int GetCoolDown()
        {
            int? value = getSkillModifiers(SkillModifierType.CoolDown, GetCoolDownInternal());
            if (value == null)
            {
                return 0;
            }
            return (int)value;
        }
        protected virtual int? GetCoolDownInternal()
        {
            return coolDown;
        }

        protected int? range = null;
        public int GetRange()
        {
            int? value = getSkillModifiers(SkillModifierType.Range, GetRangeInternal());
            if (value == null)
            {
                return 0;
            }
            return (int)value;
        }
        protected int? GetRangeInternal()
        {
            return range;
        }

        public virtual string GetTitle()
        {
            return title;
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

        public virtual List<TileSelectOption> TileOptionSet()
        {
            return new List<TileSelectOption>();
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

        protected int? getSkillModifiers(SkillModifierType type, int? baseValue)
        {
            float? value = baseValue;
            List<SkillModifier> modifiers = boardEntity.GetSkillModifier(this);


            foreach (SkillModifier mod in modifiers)
            {
                if (mod.Application == SkillModifierApplication.Add && mod.Type == type)
                {
                    value = mod.Apply(value, type);
                }
            }
            foreach (SkillModifier mod in modifiers)
            {
                if (mod.Application == SkillModifierApplication.Mult && mod.Type == type)
                {
                    value = mod.Apply(value, type);
                }
            }
            foreach (SkillModifier mod in modifiers)
            {
                // not really applying the addnomult
                if (mod.Application == SkillModifierApplication.AddNoMult && mod.Type == type)
                {
                    value = mod.Apply(value, type);
                }            
            }   

            return (int?)value;
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


        /// <summary>
        /// override this to generate damage package
        /// </summary>
        /// <returns></returns>
        protected virtual DamagePackageInternal GenerateDamagePackage()
        {

            int effectivePower = GetStrength();
            if(effectivePower != 0)
            {
                return new DamagePackageInternal(effectivePower, DamageType.physical);
            }
            return null;
        }

        public string GetDescription()
        {
            string descriptionReturn = GetDescriptionHelper();
            string descriptionExtra = GetDescriptionExtra();
            if(descriptionExtra != "")
            {
                descriptionReturn += "\n" + descriptionExtra;
            }
            return descriptionReturn;
        }

        /// <summary>
        /// override for function based description
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDescriptionHelper()
        {
            return description;
        }


        private string GetDescriptionExtra()
        {

            string returnString = "";
            returnString = GetDescriptionExtraHelper("STRENGTH: ", GetStrengthInternal, GetStrength, returnString);
            returnString = GetDescriptionExtraHelper("AP COST: ", GetAPCostInternal, GetAPCost, returnString);
            returnString = GetDescriptionExtraHelper("COOLDOWN: ", GetCoolDownInternal, GetCoolDown, returnString);
            return returnString;
            
        }

        private string GetDescriptionExtraHelper(String label, Func<int?> valueFunc, Func<int> newValueFunc, string returnString)
        {
            int? value = valueFunc();
            if(value != null)
            {
                int? newValue = newValueFunc();
                returnString += label + " " + value + " ";
                if(newValue != value)
                {
                    returnString += "-> " + newValue;
                }
                returnString += "\n";               
            }
            return returnString;
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
