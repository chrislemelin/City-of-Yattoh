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
            get { return passives; }
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
        private Action moveDoneCallback;

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
            basicAttack = new BasicAttack(tileManager, this, battleCalculator);
            skills.Add(basicAttack);
         
        }

        public override List<Move> MoveSet()
        {
            return tileManager.DFSMoves(GetTile().Position, this, team: team);
        }
        
        public List<SkillModifier> GetSkillModifier(Skill skill)
        {
            List<SkillModifier> skillModifiers = new List<SkillModifier>();
            foreach(Passive passive in passives)
            {
                skillModifiers.AddRange(passive.GetSkillModifiers(skill));
            }
            return skillModifiers;
        }

        private List<Tile> path = new List<Tile>();

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

        public void ExecuteMove(Move move, Action action = null)
        {
            if(characterAnimation != null)
            {
                characterAnimation.OnButtonClick(1);
            }
            moveDoneCallback = action;
            if(move != null)
            {
                OutlineOnHover.disabled = true;
                PathOnClick.pause = true;

                stats.SetMutableStat(StatType.Movement, move.movementPointsAfterMove);
                stats.SubtractAPPoints(move.apCost);
                path = move.path;
            }

            ChangeTarget();
        }

        private void ChangeTarget()
        {
            if(path.Count > 0)
            {
                AnimatorUtils.animationDirection dir = AnimatorUtils.GetAttackDirectionCode(GetTile().Position, path[0].Position);
                SetAnimationDirection(dir);
                target = path[0];
                path.RemoveAt(0);
            }
            else
            {
                
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
                    moveDoneCallback();
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
            foreach(Passive passive in passives)
            {
                passive.StartTurn();
                skipTurn = passive.SkipTurn(skipTurn);
            }
            if(skipTurn)
            {
                turnManager.NextTurn();
            }
            else
            {               
                if (team == Team.Enemy)
                {
                    BoardEntity boardEntity = GetRagedBy();
                    enemyAIBasic1.ExecuteTurn(turnManager.NextTurn, ragedBy:boardEntity);
                    //turnManager.NextTurn();
                }
            }           
        }

        private void ExecuteMoveHelper(Move move)
        {
            if (move != null)
            {
                //stats.SubtractMovementPoints(move.movementCost);
                stats.SetMutableStat(StatType.Movement, move.movementPointsAfterMove);
                stats.SubtractAPPoints(move.apCost);
                path = move.path;
                if (path.Count > 0)
                {
                    target = path[0];
                    path.Remove(target);
                }
            }
            else
            {
                if(moveDoneCallback != null)
                    moveDoneCallback();
            }
        }

        public void AddPassive(Passive passive)
        {
            passives.Add(passive);
        }

        public void AddSkill(Skill skill)
        {
            skills.Add(skill);
        }

        public void AddBuff(Buff buff)
        {
            buff.addRemoveAction(passives.Remove);          
        }


        public CharacterBoardEntity GetRagedBy()
        {
            CharacterBoardEntity returnEntity = null;
            foreach(Passive passive in passives)
            {
                returnEntity = passive.GetRagedBy(returnEntity);
            }
            return returnEntity;
        }

        public HashSet<Tile> GetTauntTiles()
        {
            HashSet<Tile> returnTauntTiles = new HashSet<Tile>();
            foreach (Passive passive in passives)
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
            foreach(Passive p in passives)
            {
                stealthed = p.IsStealthed(stealthed);
            }
            return stealthed;
        }

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
