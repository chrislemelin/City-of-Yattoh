using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
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
        protected TurnManager turnManager;
        protected AnimatorUtils.animationType animationType = AnimatorUtils.animationType.attack;
        protected string flavorText;
        protected bool targetSelfTeam = false;
        protected DamageType damageType = DamageType.physical;

        private List<SkillModifier> skillModifiers = new List<SkillModifier>();

        [SerializeField]
        protected string description = "CHANGE THE SKILL DESCRIPTION PLS";

        protected int currentCoolDown = 0;

        protected List<Buff> buffs = new List<Buff>();
        public virtual List<Buff> GetBuffs()
        {
            return new List<Buff>(buffs);
        }

        protected int? apCost = null;
        public int GetAPCost()
        {
            int? value = GetStatAfterSkillModifiers(SkillModifierType.APCost, GetAPCostInternal());
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
            int? value = GetStatAfterSkillModifiers(SkillModifierType.Power, GetStrengthInternal());
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
            int? value = GetStatAfterSkillModifiers(SkillModifierType.Piercing, GetPiercingInternal());
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
            int? value = GetStatAfterSkillModifiers(SkillModifierType.CoolDown, GetCoolDownInternal());
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
        protected int? range = 1;
        public int GetRange()
        {
       
            int? value = GetStatAfterSkillModifiers(SkillModifierType.Range, GetRangeInternal());
            if(GetRangeInternal() == RANGE_ADJACENT)
            {
                if (GetSkillModifiers(SkillModifierType.Range).Count > 0)
                    return ((int)GetStatAfterSkillModifiers(SkillModifierType.Range, 1));
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

        public Skill()
        {
        }

        public bool SelfCasting()
        {
            return GetRangeInternal() == RANGE_SELF;
        }    

        public void Init(TileManager tileManager, CharacterBoardEntity boardEntity, BattleCalculator battleCalculator, TurnManager turnManager)
        {
            this.tileManager = tileManager;
            this.boardEntity = boardEntity;
            this.battleCalculator = battleCalculator;
            this.turnManager = turnManager;
        }

        public void PartialInit(CharacterBoardEntity boardEntity)
        {
            this.boardEntity = boardEntity;
        }

        public void StartTurn()
        {
            //ReduceCooldowns();
        }

        public void EndTurn()
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
                SkillReport skillReport = null;
                if(clickable)
                {
                    skillReport = TheoreticalAction(t);
                }
                Stats targetAfter = null;
                if (clickable && skillReport != null)
                    targetAfter = skillReport.TargetAfter;
                tileOptions.Add(new TileSelectOption
                {
                    Selection = t,
                    OnHover = TileReturnHelper(t),
                    HighlightColor = selectColor,
                    HoverColor = highlightColor,
                    DisplayStats = targetAfter,
                    skillReport = skillReport,
                    Clickable = TileOptionClickable(t),
                    ReturnObject = TileReturnHelper(t)
                   
                });
            }
           

            return tileOptions;
        }

        protected virtual SkillReport GetSkillReport(Tile t)
        {
            List<DamagePackage> packages = GenerateDamagePackages();
            if (t.BoardEntity != null)
            {
                return battleCalculator.ExecuteSkillHelper(boardEntity, this, (CharacterBoardEntity)t.BoardEntity,
                    packages);
            }
            return null;
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
                return TileHasTarget(t);
            }
        }

        protected virtual bool TileHasTarget(Tile t)
        {
            if(!targetSelfTeam)
                return t.BoardEntity != null && t.BoardEntity.Team != boardEntity.Team;
            else
                return t.BoardEntity != null && t.BoardEntity.Team == boardEntity.Team;
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
            if(GetRange() == RANGE_ADJACENT)
            {
                return tileManager.GetTilesDiag(p, 1);
            }
            else
            {
                return tileManager.GetTilesNoDiag(p, GetRange());

            }

        }

        public void Action(Tile t, Action<bool> callback = null, bool free = false)
        {
            Action(new List<Tile>() { t }, callback);
        }

        public void Action(List<Tile> tiles, Action<bool> callback = null, bool free = false, List<SkillModifier> mods = null, bool animation = true)
        {
            if( mods != null )
            {
                skillModifiers = mods;
            }
            if (tiles.Count > 0)
            {
                boardEntity.SetAnimationDirection(AnimatorUtils.GetAttackDirectionCode(boardEntity.GetTile().Position, tiles[0].Position));
                if(animationType != AnimatorUtils.animationType.none && animation) 
                {
                    boardEntity.SetAnimation(animationType);
                }
            }

            SkillReport report = ActionHelper(tiles);
            if(report != null)
            {
                foreach (Tuple<Stats, Stats> stat in report.targets)
                {
                    foreach (Passive p in ((CharacterBoardEntity)stat.first.BoardEntity).Passives)
                    {
                        p.AttackedBy(boardEntity);
                    }
                }
            }
      

            ActionHelperNoPreview(tiles, callback);
            battleCalculator.ExecuteSkillReport(report);
            foreach(Tile t in tiles)
            {
                if(TileHasTarget(t))
                {
                    foreach (Buff b in GetBuffs())
                    {
                        ((CharacterBoardEntity)t.BoardEntity).AddPassive(b);
                    }
                }
            }

            if(!free)
            {
                currentCoolDown = GetCoolDown();
                boardEntity.Stats.SubtractAPPoints(GetAPCost());
            }

            // tell the passives what just happened
            foreach (Passive passive in boardEntity.Passives)
            {
                passive.ExecutedSkillFast(this, report);
            }

            // tell the passives what just happened
            foreach (Passive passive in boardEntity.Passives)
            {
                passive.ExecutedSkill(this, report);
            }
         
            skillModifiers = new List<SkillModifier>();
            turnManager.CheckEntitiesForDeath();

            DoCallback(callback);
        }

        protected virtual void DoCallback(Action<bool> callback = null)
        {
            if (callback != null)
            {
                Core.CallbackDelay(.8f, () => callback(false));
            }
        }

        /// <summary>
        /// override for the actual execution of skill
        /// </summary>
        /// <param name="t"></param>
        protected abstract SkillReport ActionHelper(List<Tile> t);

        protected virtual void ActionHelperNoPreview(List<Tile> tiles, Action<bool> calback = null){ }

        public virtual bool IsActive()
        {
            return CanAffortAPCost() && currentCoolDown == 0 && !PathOnClick.pause;
        }

        public SkillReport TheoreticalAction(Tile t)
        {
            return ActionHelper(new List<Tile>() { t });
            /*
            DamagePackage package = GenerateDamagePackage();
            SkillReport report = battleCalculator.ExecuteSkillHelper(boardEntity, this, (CharacterBoardEntity)t.BoardEntity,
                new List<DamagePackage>() { package });
            report.Buffs.AddRange(GetBuffs());
            return report;
            */
        }

        protected List<SkillModifier> GetSkillModifiers(SkillModifierType type)
        {
            List<SkillModifier> returnModifiers = new List<SkillModifier>();
            List<SkillModifier> modifiers = boardEntity.GetSkillModifier(this);
            modifiers.AddRange(skillModifiers);
            foreach(SkillModifier mod in modifiers)
            {
                if(mod.Type == type)
                {
                    returnModifiers.Add(mod);
                }
            }
            return returnModifiers;
        }

        protected int? GetStatAfterSkillModifiers(SkillModifierType type, int? baseValue)
        {
            float? value = baseValue;
            List<SkillModifier> modifiers = GetSkillModifiers(type);

            foreach (SkillModifier mod in modifiers)
            {
                if (mod.Application == SkillModifierApplication.Add)
                {
                    value = mod.Apply(value, type);
                }
            }

            foreach (SkillModifier mod in modifiers)
            {
                if (mod.Application == SkillModifierApplication.Mult)
                {
                    value = mod.Apply(value, type);
                }
            }
            foreach (SkillModifier mod in modifiers)
            {
                // not really applying the addnomult
                if (mod.Application == SkillModifierApplication.AddNoMult)
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
        public virtual DamagePackage GenerateDamagePackage()
        {
            int effectivePower = GetStrength();
            int effectivePiercing = GetPiercing();
 
            return new DamagePackage(effectivePower, damageType , effectivePiercing);
            
        }

        public virtual List<DamagePackage> GenerateDamagePackages()
        {
            List<DamagePackage> packages = new List<DamagePackage>();
            packages.Add(GenerateDamagePackage());
            foreach(Passive p in boardEntity.Passives)
            {
                packages.AddRange(p.GetDamagePackage(this));
            }
            return packages;
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
        public virtual string GetDescriptionHelper()
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

    public enum DamageType { physical, pure, armour };

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
