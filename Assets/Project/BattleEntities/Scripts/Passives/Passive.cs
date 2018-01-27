using UnityEngine;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Env;

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

        protected TakeDamageReturn takeDamageReturn = TakeDamageReturn.Normal;

        // default skillModifiers for ALL skills
        protected List<SkillModifier> skillModifiers = new List<SkillModifier>();

        // if passive should cause the turn to be skipped
        protected bool skip = false;

        public enum PassiveType { None, Buff, Debuff, TalentTrigger, Talent}

        protected PassiveType type;
        public PassiveType Type
        {
            get { return type; }
        }

        public Passive()
        {
        }

        public Passive(BattleCalculator battleCalculator, CharacterBoardEntity boardEntity, TileManager tileManager)
        {
            this.battleCalculator = battleCalculator;
            this.boardEntity = boardEntity;
            this.tileManager = tileManager;
        }

        /// <summary>
        /// for things that cannot be expressed by the skill modifiers, just directly change the damage package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public virtual TakeDamageReturn TakeDamage(Skill skill, DamagePackage package, TakeDamageReturn lastReturn)
        {
            return (TakeDamageReturn)Mathf.Max((int)takeDamageReturn, (int)lastReturn);
        }

        public bool SkipTurn(bool skip)
        {
            return this.skip || skip;
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
        public virtual void ExecutedSkill(SkillReport skillreport)
        {

        }

        public virtual string GetTitle()
        {
            return title;
        }
        
        public virtual string GetDescription()
        {
            return description;
        }

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

        protected virtual List<SkillModifier> GetSkillHelperModifiers()
        {
            return skillModifiers;
        }       

        public virtual void StartTurn(){}

        public virtual void LeaveTile(Tile t){}

        public virtual void EnterTile(Tile t){}

        public virtual HashSet<Tile> GetTauntTiles() { return new HashSet<Tile>();}

        public virtual bool IsStealthed(bool stealthed)
        {
            return stealthed;
        }

        public virtual CharacterBoardEntity GetRagedBy(CharacterBoardEntity characterBoardEntity)
        {
            return characterBoardEntity;
        }

    }

   
}