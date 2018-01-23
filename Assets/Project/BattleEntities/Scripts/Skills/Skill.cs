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
        protected string description = "CHANGE THE SKILL DESCRIPTION PLS";

        protected int currentCoolDown = 0;

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

        public readonly int? RANGE_SELF = null;
        public readonly int? RANGE_ADJACENT = int.MinValue;
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

        public Skill(TileManager tileManager, CharacterBoardEntity boardEntity, BattleCalculator battleCalculator)
        {
            this.tileManager = tileManager;
            this.boardEntity = boardEntity;
            this.battleCalculator = battleCalculator;
        }

        public void StartTurn()
        {
            ReduceCooldowns();
        }

        public void ReduceCooldowns()
        {
            if (currentCoolDown > 0)
                currentCoolDown--;
        }

        public List<Tile> TileSet()
        {
            return TileSetHelper(boardEntity.GetTile().Position);
        }

        public virtual List<TileSelectOption> TileOptionSet()
        {
            List<Tile> tiles = TileSetHelper(boardEntity.Position);
            HashSet<Tile> usedTiles = new HashSet<Tile>();
            List<TileSelectOption> tileOptions = new List<TileSelectOption>();
            foreach (Tile t in tiles)
            {
                usedTiles.Add(t);
                SkillReport report = GetSkillReport(t);
                tileOptions.Add(new TileSelectOption
                {
                    Selection = t,
                    OnHover = new List<Tile>() { t },
                    HighlightColor = selectColor,
                    HoverColor = highlightColor,
                    DisplayStats = report.TargetAfter,
                });
            }
            foreach (Tile t in TileSetPossible(boardEntity.Position))
            {
                if (!usedTiles.Contains(t))
                {
                    tileOptions.Add(new TileSelectOption
                    {
                        Selection = t,
                        OnHover = new List<Tile>() { t },
                        HighlightColor = selectColor,
                        HoverColor = highlightColor,
                        Clickable = false
                    });
                }
            }

            return tileOptions;
        }

        protected SkillReport GetSkillReport(Tile t)
        {
            DamagePackageInternal damagePackage = GenerateDamagePackage();
            DamagePackage package = new DamagePackage(damagePackage);
            return battleCalculator.ExecuteSkillHelper(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, package);
        } 

        public List<Tile> TheoreticalTileSet(Position p)
        {
            return TileSetHelper(p);
        }

        /// <summary>
        /// override for tile set for which tiles the move can be used
        /// </summary>
        /// <returns></returns>
        public virtual List<Tile> TileSetHelper(Position p)
        {
            if(range == RANGE_SELF)
            {
                return TileSetPossible(p);
            }
            else
            {
                return TeamTiles(TileSetPossible(p), boardEntity.Team);
            }
        }


        /// <summary>
        /// which tiles set the skill CAN be casted on ignoring who is on the tile
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual List<Tile> TileSetPossible(Position p)
        {
            if(range == RANGE_SELF)
            {
                List<Tile> returnTiles = new List<Tile>();
                returnTiles.Add(tileManager.GetTile(p));
                return returnTiles;
            }
            if(range == RANGE_ADJACENT)
            {
                return tileManager.GetAllTilesDiag(p, 1);
            }
            else
            {
                return tileManager.GetTilesNoDiag(p, GetRange());

            }

        }

        public void Action(Tile t, Action callback = null)
        {
            ActionHelper(t);
            currentCoolDown = GetCoolDown();
            boardEntity.Stats.SubtractAPPoints(GetAPCost());

            if (callback != null)
            {
                Core.CallbackDelay(.8f, callback);                
            }
        }

        /// <summary>
        /// override for the actual execution of skill
        /// </summary>
        /// <param name="t"></param>
        protected abstract void ActionHelper(Tile t);


        public virtual bool IsActive()
        {
            return TileSet().Count > 0 && CanAffortAPCost() && currentCoolDown == 0;
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
                descriptionReturn += "\n\n" + descriptionExtra;
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

            if (currentCoolDown != 0)
            {
                returnString += "availible in " + currentCoolDown + " turns \n";
            }

            returnString = GetDescriptionExtraHelper("STRENGTH: ", GetStrengthInternal, GetStrength, returnString);
            returnString = GetDescriptionExtraHelper("AP COST: ", GetAPCostInternal, GetAPCost, returnString);
            returnString = GetDescriptionExtraHelper("COOLDOWN: ", GetCoolDownInternal, GetCoolDown, returnString);
            returnString = GetDescriptionExtraHelper("RANGE: ", GetRangeInternal, GetRange, returnString);
            
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
