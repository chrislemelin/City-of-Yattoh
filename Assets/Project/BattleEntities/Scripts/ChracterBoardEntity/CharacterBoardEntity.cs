using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.AI;
using Placeholdernamespace.Common.Animator;

namespace Placeholdernamespace.Battle.Entities
{
    public class CharacterBoardEntity : BoardEntity
    {
        [SerializeField]
        protected CharacterAnimation characterAnimation;
        [SerializeField]
        protected GameObject charactersprite;

        private static List<CharacterBoardEntity> allCharacterBoardEntities = new List<CharacterBoardEntity>();
        public static List<CharacterBoardEntity> AllCharacterBoardEntities
        {
            get { return new List<CharacterBoardEntity>(allCharacterBoardEntities); }
        }

        [SerializeField]
        private float speed = 5;

        protected int? range = Skill.RANGE_ADJACENT;
        public int? Range
        {
            get { return range; }
        }

        protected List<Passive> passives = new List<Passive>();
        public List<Passive> Passives
        {
            get { return new List<Passive>(passives); }
        }

        protected List<Talent> talents = new List<Talent>();
        public List<Talent> Talents
        {
            get { return talents; }
        }

        protected List<TalentTrigger> talentTriggers = new List<TalentTrigger>();
        public List<TalentTrigger> TalentTriggers
        {
            get { return talentTriggers; }
        }

        private BasicAttack basicAttack;
        public BasicAttack BasicAttack
        {
            get { return basicAttack; }
        }

        [SerializeField]
        private EnemyAIBasic enemyAIBasic1;

        //private SkillSelector skillSelector;
        private Tile target = null;
        
        private Dictionary<Tile, Move> cachedMoves = new Dictionary<Tile, Move>();
        private Action<bool> moveDoneCallback;


        public override void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            base.Init(startingPosition, turnManager, tileManager, boardEntitySelector, battleCalculator);

            if (charactersprite != null)
            {
                charactersprite.transform.SetParent(FindObjectOfType<CharacterManagerMarker>().transform);
            }

            allCharacterBoardEntities.Add(this);
            if(enemyAIBasic1 != null)
            {
                enemyAIBasic1.Init(tileManager, this);
            }

            basicAttack = new BasicAttack();
            AddSkill(basicAttack);
            /*
            basicAttack = new BasicAttack(tileManager, this, battleCalculator);
            skills.Add(basicAttack);
            basicAttack = new BasicAttack(tileManager, this, battleCalculator);
            skills.Add(basicAttack);
            basicAttack = new BasicAttack(tileManager, this, battleCalculator);
            skills.Add(basicAttack);
            basicAttack = new BasicAttack(tileManager, this, battleCalculator);
            skills.Add(basicAttack);
           */

        }

        public override List<Move> MoveSet()
        {
            return tileManager.DFSMoves(GetTile().Position, this, team: team, tauntTiles:GetTauntTiles());
        }
        
        public List<SkillModifier> GetSkillModifier(Skill skill)
        {
            List<SkillModifier> skillModifiers = new List<SkillModifier>();
            foreach(Passive passive in Passives)
            {
                skillModifiers.AddRange(passive.GetSkillModifiers(skill));
            }
            return skillModifiers;
        }

        public List<StatModifier> GetStatModifiers()
        {
            List<StatModifier> statModifiers = new List<StatModifier>();
            foreach(Passive passive in Passives)
            {
                statModifiers.AddRange(passive.GetStatModifiers());
            }
            return statModifiers;
        }

        private List<Tile> path = new List<Tile>();
        private int pathCounter = 0;
        private bool interupted = false;

        private void checkAtTarget()
        {
            if (transform.position == target.transform.position)
            {
                Tile leavingTile = GetTile();
                tileManager.MoveBoardEntity(target.Position, this);
                Tile tempTarget = target;
                target = null;
                tempTarget.ExecuteEnterActions(this, leavingTile, ChangeTarget);
            }
        }

        private void doMovement()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }

        public void ExecuteMove(Move move, Action<bool> action = null)
        {
            interupted = false;
            pathCounter = 0;
            if(characterAnimation != null)
            {
                characterAnimation.OnButtonClick(1);
            }
            moveDoneCallback = action;
            if(move != null)
            {
                OutlineOnHover.disabled = true;
                PathOnClick.pause = true;

                foreach(Passive p in Passives)
                {
                    p.ExecutedMove(move);
                }

                //stats.SetMutableStat(StatType.Movement, move.movementPointsAfterMove);
                //stats.SubtractAPPoints(move.apCost);
                path = move.path;
            }
            ChangeTarget();
        }

        private void ChangeTarget()
        {

            // we gotta check to see if we just walked into a taunt, we will have to do this for taunt as well
            if (GetTauntTiles().Count != 0)
            {
                HashSet<Tile> tauntTiles = GetTauntTiles();
                for(int a = 0; a < path.Count; a++)
                {
                    if (tauntTiles.Contains(path[a]))
                    {
                        path.RemoveRange(a, path.Count - a);
                        interupted = true;
                    }
                }
            }

            if(path.Count > 0)
            {
                AnimatorUtils.animationDirection dir = AnimatorUtils.GetAttackDirectionCode(GetTile().Position, path[0].Position);
                SetAnimationDirection(dir);
                target = path[0];
                path.RemoveAt(0);
                pathCounter++;

            }
            else
            {
                stats.SubtractMovementPoints(pathCounter);

                // all done moving
                target = null;
                PathOnClick.pause = false;
                OutlineOnHover.disabled = false;
                if (moveDoneCallback != null)
                {
                    if(characterAnimation != null)
                    {
                        characterAnimation.OnButtonClick(0);
                    }
                    moveDoneCallback(interupted);
                }
            }
        }

        void Update()
        {
            if (target != null)
            {
                doMovement();
                checkAtTarget();
            }
        }

        public override void StartMyTurn()
        {
            stats.NewTurn();
            bool skipTurn = false;
            foreach(Skill skill in skills)
            {
                skill.StartTurn();
            }
            foreach(Passive passive in Passives)
            {
                passive.StartTurn();
                skipTurn = passive.SkipTurn(skipTurn);

            }
            if (skipTurn)
            {
                EndMyTurn();
            }
            else
            {               

                if (team == Team.Enemy)
                {
                    BoardEntity boardEntity = GetRagedBy();
                    enemyAIBasic1.ExecuteTurn(this, EndMyTurn, ragedBy:boardEntity);
                }
            }           
        } 
        
        public void EndMyTurn()
        {
            foreach(Passive p in Passives)
            {
                p.EndTurn();
            }
            foreach (Skill skill in Skills)
            {
                skill.EndTurn();
            }
            turnManager.NextTurn();
        }

        public void ReduceCooldowns()
        {
            foreach(Skill skill in skills)
            {
                skill.ReduceCooldowns();
            }
        }

        // Passives

        /// <summary>
        /// please use this when adding any type of passive
        /// </summary>
        /// <param name="passive"></param>
        public void AddPassive(Passive passive)
        {
            passive.Init(battleCalculator, this, tileManager);
            bool add = true;
            if (passive is Buff)
            {
                add = AddBuff((Buff)passive);
            }
            if (passive is TalentTrigger)
            {
                TalentTriggers.Add((TalentTrigger)passive);
            }
            if (passive is Talent)
            {
                Talents.Add((Talent)passive);
            }
            if(add)
                passives.Add(passive);

            
        }

        public void RemoveBuff<buffClass>()
        {
            foreach(Passive p in Passives)
            {
                if(p is Buff)
                {
                    if(p is buffClass)
                    {
                        ((Buff)p).PopAll();
                    }
                }
            }
        }

        public void TriggerTalents()
        {
            foreach(Talent talent in talents)
            {
                talent.Activate();
            }
        }

        public bool HasPassiveType(PassiveType type)
        {
            foreach (Passive p in passives)
            {
                if(p.Type == type)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddSkill(Skill skill)
        {
            skill.Init(tileManager, this, battleCalculator);
            skills.Add(skill);
        }

        protected bool AddBuff(Buff buff)
        {
            buff.Init(passives.Remove);
            foreach(Passive p in passives)
            {
                if(buff.GetType() == p.GetType())
                {
                    ((Buff)p).AddSameBuff(buff);
                    return false;
                }
            }
            return true;
        }

        public CharacterBoardEntity GetRagedBy()
        {
            CharacterBoardEntity returnEntity = null;
            foreach(Passive passive in Passives)
            {
                returnEntity = passive.GetRagedBy(returnEntity);
            }
            return returnEntity;
        }

        public HashSet<Tile> GetTauntTiles()
        {
            HashSet<Tile> returnTauntTiles = new HashSet<Tile>();
            foreach (Passive passive in Passives)
            {
                foreach(Tile tile in passive.GetTauntTiles())
                {
                    returnTauntTiles.Add(tile);
                }
            }
            return returnTauntTiles;
        }

        public bool IsStealthed()
        {
            bool stealthed = false;
            foreach(Passive p in Passives)
            {
                stealthed = p.IsStealthed(stealthed);
            }
            return stealthed;
        }

        // ANIMATIONS

        public void SetAnimation(AnimatorUtils.animationType type)
        {
            if (characterAnimation != null)
            {
                characterAnimation.OnButtonClick((int)type);
            }
        }

        private AnimatorUtils.animationDirection? lastDirection = AnimatorUtils.animationDirection.right;

        public bool ChangeDirection(AnimatorUtils.animationDirection direction)
        {
            HashSet<AnimatorUtils.animationDirection> noRotate = new HashSet<AnimatorUtils.animationDirection>
            {
                AnimatorUtils.animationDirection.right, AnimatorUtils.animationDirection.down
            };
            if(lastDirection == direction)
            {
                return true;
            }
            if (lastDirection != null)
            {
                if (noRotate.Contains((AnimatorUtils.animationDirection)lastDirection) && noRotate.Contains(direction))
                    return true;
            }
            
            return false;
        }

        public void SetAnimationDirection(AnimatorUtils.animationDirection direction)
        {
            if (characterAnimation != null)
            {
                //bool changeDirection = changeDirection(direction);
                if (direction == AnimatorUtils.animationDirection.left)
                {
                    characterAnimation.On_Front_Back(true);
                    characterAnimation.On_Left_Right(true);
                }
                else if (direction == AnimatorUtils.animationDirection.right)
                {
                    characterAnimation.On_Front_Back(false);
                    characterAnimation.On_Left_Right(false);
                    
                }
                else if (direction == AnimatorUtils.animationDirection.up)
                {
                    characterAnimation.On_Front_Back(false);
                    characterAnimation.On_Left_Right(true);
                }
                else if (direction == AnimatorUtils.animationDirection.down)
                {
                    characterAnimation.On_Front_Back(true);
                    characterAnimation.On_Left_Right(false);
                }
                lastDirection = direction;
            }
        }

      
    }


 
  
    
}
