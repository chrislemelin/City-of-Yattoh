using UnityEngine;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Entities.AttributeStats;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public abstract class Passive
    {
        protected BattleCalculator battleCalculator;
        protected CharacterBoardEntity boardEntity;
        protected TileManager tileManager;

        protected string title;
        protected string description = "CHANGE THE PASSIVE DESCRIPTION PLS";
        protected Color displayColor;

        // default skillModifiers for ALL skills
        protected List<SkillModifier> skillModifiers = new List<SkillModifier>();

        protected List<StatModifier> statModifiers = new List<StatModifier>();

        // if passive should cause the turn to be skipped
        protected bool skip = false;

        protected bool stealthed = false;

        // if this character should go first in the order queue
        protected bool turnOrderFirst = false;

        protected PassiveType type;
        public PassiveType Type
        {
            get { return type; }
        }

        public Passive()
        {
            type = PassiveType.Passive;
        }

        public virtual void Init(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager)
        {
            this.battleCalculator = battleCalculator;
            this.boardEntity = boardEntity;
            this.tileManager = tileManager;
        }

        public void PartialInit(CharacterBoardEntity boardEntity)
        {
            this.boardEntity = boardEntity;
        }

        /// <summary>
        /// for things that cannot be expressed by the skill modifiers, just directly change the damage package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public virtual TakeDamageReturn TakeDamage(Skill skill, DamagePackage package, TakeDamageReturn lastReturn)
        {
            return lastReturn;
        }

        public bool SkipTurn(bool skip)
        {
            return this.skip || skip;
        }

        public bool TurnOrderFirst(bool turnOrderFirst)
        {
            return this.turnOrderFirst || turnOrderFirst;
        }

        public bool IsType(bool isType, PassiveType type)
        {
            if(this.type == type)
            {
                return true;
            }
            return isType;
        }

        /// <summary>
        /// for things that cannot be expressed by the skill modifiers, just directly change the damage package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public virtual DamagePackage DoDamage(DamagePackage package)
        {
            return package;
        }

        /// <summary>
        /// when a skill is executed
        /// </summary>
        /// <param name="skillreport"></param>
        public virtual void ExecutedSkill(Skill skill, SkillReport skillreport){ }

        public virtual void ExecutedSkillFast(Skill skill, SkillReport skillReport){ }

        public virtual string GetTitle()
        {
            return GetTitleHelper() + " (PASSIVE)";
        }

        public string GetTitleHelper()
        {
            return title;
        }
        
        public virtual string GetDescription()
        {
            return "Passive: " + GetDescriptionHelper();
        }

        public string GetDescriptionHelper()
        {
            return description;
        }

        public virtual void StartBattle() { }

        public virtual void AttackedBy(CharacterBoardEntity character) { }

        public List<SkillModifier> GetSkillModifiers(Skill skill)
        {
            List<SkillModifier> modifiers = GetSkillHelperModifiers();
            modifiers.AddRange(GetSkillHelperModifiers(skill));
            return modifiers;
        }

        protected virtual List<SkillModifier> GetSkillHelperModifiers(Skill skill)
        {
            return new List<SkillModifier>();
        }

        protected List<SkillModifier> GetSkillHelperModifiers()
        {
            return new List<SkillModifier>(skillModifiers); ;
        }

        public List<StatModifier> GetStatModifiers()
        {
            List<StatModifier> modifiers = GetStatHelperModifiersDefault();
            modifiers.AddRange(GetStatHelperModifiers());
            return modifiers;
        }

        protected virtual List<StatModifier> GetStatHelperModifiers()
        {
            return new List<StatModifier>();
        }

        protected List<StatModifier> GetStatHelperModifiersDefault()
        {
            return new List<StatModifier>(statModifiers);
        }

        public virtual void Die(){}

        public virtual void StartTurn(){}

        public virtual void EndTurn(){}

        public virtual void LeaveTile(Tile t){}

        public virtual void EnterTile(Tile t){}

        public virtual HashSet<Tile> GetTauntTiles() { return new HashSet<Tile>();}

        public virtual bool IsStealthed(bool stealthed)
        {
            return this.stealthed || stealthed;
        }

        public virtual List<DamagePackage> GetDamagePackage(Skill skill)
        {
            return new List<DamagePackage>();
        }

        public virtual void AboutToExecuteAction(Skill skill, List<Tile> tile){}

        public virtual void ExecutedMove(Move move){}

        public virtual CharacterBoardEntity GetRagedBy(CharacterBoardEntity characterBoardEntity)
        {
            return characterBoardEntity;
        }

        

    }

    public enum PassiveType { Passive, Buff, Debuff, TalentTrigger, Talent }

}