using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.UI;
using Placeholdernamespace.Common.Animator;
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
        protected AnimatorUtils.animationType animationType = AnimatorUtils.animationType.attack;
        protected string flavorText;

        [SerializeField]
        protected string description = "CHANGE THE SKILL DESCRIPTION PLS";

        protected int currentCoolDown = 0;

        protected int? apCost = null;
        public int GetAPCost()
        {
            int? value = getSkillModifiers(SkillModifierType.APCost, GetAPCostInternal());
            if (value == null)
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

        protected int? piercing = null;
        public int GetPiercing()
        { 
            int? value = getSkillModifiers(SkillModifierType.Piercing, GetPiercingInternal());
            if (value == null)
            {
                return 0;
            }
            return (int) value;
        }
        protected virtual int? GetPiercingInternal()
        {
            return piercing;
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

        public static readonly int? RANGE_SELF = null;
        public static readonly int? RANGE_ADJACENT = int.MinValue;
        protected int? range = null;
        public int GetRange()
        {
       
            int? value = getSkillModifiers(SkillModifierType.Range, GetRangeInternal());
            if(GetRangeInternal() == RANGE_ADJACENT)
            {
                if (value > 0)
                    return ((int)getSkillModifiers(SkillModifierType.Range, 1));
                else
                    return (int)RANGE_ADJACENT;
            }
            if (value == null)
            {
                return 0;
            }
            return (int)value;
        }
        protected virtual int? GetRangeInternal()
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
                bool clickable = TileOptionClickable(t);
                Stats targetAfter = null;
                if (clickable)
                    targetAfter = GetSkillReport(t).TargetAfter;
                tileOptions.Add(new TileSelectOption
                {
                    Selection = t,
                    OnHover = TileReturnHelper(t),
                    HighlightColor = selectColor,
                    HoverColor = highlightColor,
                    DisplayStats = targetAfter,
                    Clickable = TileOptionClickable(t),
                    ReturnObject = TileReturnHelper(t)
                   
                });
            }
           

            return tileOptions;
        }

        protected SkillReport GetSkillReport(Tile t)
        {
            DamagePackage package = GenerateDamagePackage();
            return battleCalculator.ExecuteSkillHelper(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, package);
        } 

        public List<Tile> TheoreticalTileSet(Position p)
        {
            return TileSetHelper(p);
        }
        
        public List<Tile> TileSetClickable(Position p)
        {
            List<Tile> tiles = TileSetHelper(p);
            List<Tile> newTiles = new List<Tile>();
            foreach(Tile t in tiles)
            {
                if(TileOptionClickable(t))
                {
                    newTiles.Add(t);
                }
            }
            return newTiles;
        }

        public virtual bool TileOptionClickable(Tile t )
        {
            if(t == tileManager.GetTile(boardEntity.Position) && range == RANGE_SELF)
            {
                return true;
            }
            else
            {
                return t.BoardEntity != null && t.BoardEntity.Team != boardEntity.Team;
            }
        }

        public virtual List<Tile> TileReturnHelper(Tile t)
        {
            return new List<Tile>(){ t };

   
        }

        /// <summary>
        /// which tiles set the skill CAN be casted on ignoring who is on the tile
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual List<Tile> TileSetHelper(Position p)
        {
            if(GetRangeInternal() ==  RANGE_SELF)
            {
                List<Tile> returnTiles = new List<Tile>();
                returnTiles.Add(tileManager.GetTile(p));
                return returnTiles;
            }
            if(GetRangeInternal() == RANGE_ADJACENT)
            {
                return tileManager.GetTilesDiag(p, 1);
            }
            else
            {
                return tileManager.GetTilesNoDiag(p, GetRange());

            }

        }

        public void Action(Tile t, Action<bool> callback = null)
        {
            Action(new List<Tile>() { t }, callback);
        }

        public void Action(List<Tile> tiles, Action<bool> callback = null)
        {
            if (tiles.Count > 0)
            {
                boardEntity.SetAnimationDirection(AnimatorUtils.GetAttackDirectionCode(boardEntity.GetTile().Position, tiles[0].Position));
                boardEntity.SetAnimation(AnimatorUtils.animationType.attack);
                ActionHelper(tiles);
                currentCoolDown = GetCoolDown();
                boardEntity.Stats.SubtractAPPoints(GetAPCost());
            }
            if (callback != null)
            {
                Core.CallbackDelay(.8f, () => callback(false));                
            }
        }

        /// <summary>
        /// override for the actual execution of skill
        /// </summary>
        /// <param name="t"></param>
        protected abstract void ActionHelper(List<Tile> t);

        public virtual bool IsActive()
        {
            return CanAffortAPCost() && currentCoolDown == 0;
        }

        public SkillReport TheoreticalAction(Tile t)
        {
            DamagePackage package = GenerateDamagePackage();
            SkillReport report = battleCalculator.ExecuteSkillHelper(boardEntity, this, (CharacterBoardEntity)t.BoardEntity, package);
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

        protected List<BoardEntity> TeamTiles(List<Tile> tiles, Team? team)
        {
            List<BoardEntity> entities = new List<BoardEntity>();
            foreach(Tile t in tiles)
            {
                if(t.BoardEntity != null && (team == null || t.BoardEntity.Team == team))
                    entities.Add(t.BoardEntity);
            }
            return entities;
        }

        protected bool CanAffortAPCost()
        {
            return (boardEntity.Stats.GetMutableStat(AttributeStats.StatType.AP).Value >= GetAPCost());
        }


        /// <summary>
        /// override this to generate damage package
        /// </summary>
        /// <returns></returns>
        protected virtual DamagePackage GenerateDamagePackage()
        {
            int effectivePower = GetStrength();
            int effectivePiercing = GetPiercing();
 
            return new DamagePackage(effectivePower, DamageType.physical , effectivePiercing);
            
        }

        public string GetFlavorText()
        {
            return flavorText;
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
            returnString = GetDescriptionExtraHelper("PIERCING: ", GetPiercingInternal, GetPiercing, returnString);


            if (GetRange() == RANGE_ADJACENT)
            {
                returnString += "RANGE: Adjacent\n";
            }
            else
            {
                returnString = GetDescriptionExtraHelper("RANGE: ", GetRangeInternal, GetRange, returnString);
            }

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

        public Team OtherTeam()
        {
            if(boardEntity.Team == Team.Enemy)
            {
                return Team.Player;
            }
            else if(boardEntity.Team == Team.Player)
            {
                return Team.Enemy;
            }
            return Team.Neutral;
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

        private float piercing;
        public float Piercing
        {
            get { return piercing; }
        }

        public DamagePackageInternal(float damage, DamageType type, float piercing = 0)
        {
            this.damage = damage;
            this.type = type;
            this.piercing = piercing;
        }

     

    }


}
