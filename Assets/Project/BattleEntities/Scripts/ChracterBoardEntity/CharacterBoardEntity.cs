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

namespace Placeholdernamespace.Battle.Entities
{
    public class CharacterBoardEntity : BoardEntity
    {
        [SerializeField]
        private float speed = 5;

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
        private EnemyAIBasic enemyAIBasic;

        //private SkillSelector skillSelector;
        private Tile target = null;
        private List<Tile> path;
        private Dictionary<Tile, Move> cachedMoves = new Dictionary<Tile, Move>();
        private Action moveDoneCallback;

        public void Init(TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            base.Init(turnManager, tileManager, boardEntitySelector, battleCalculator);
            if(enemyAIBasic != null)
            {
                enemyAIBasic.Init(tileManager, this);
            }
            basicAttack = new BasicAttack();
            basicAttack.Init(tileManager, this, battleCalculator);
            skills.Add(basicAttack);


            List<SkillModifier> skillModifiers = new List<SkillModifier>();
            skillModifiers.Add(new SkillModifier(SkillModifierType.Power, SkillModifierApplication.Add, 1));
            passives.Add(new PassiveGeneric("Damage Buff", "Increases damage on skills by one", skillModifiers));
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

        private void checkAtTarget()
        {
            if (transform.position == target.transform.position)
            {
                tileManager.MoveBoardEntity(target.Position, this);

                if (path.Count == 0)
                {
                    target = null;
                    PathOnClick.pause = false;
                    OutlineOnHover.disabled = false;
                    if (moveDoneCallback != null)
                    {
                        moveDoneCallback();
                    }
                }
                else
                {
                    target = path[0];
                    path.Remove(target);
                }
            }
        }

        private void doMovement()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }

        public void ExecuteMove(Move move, Action action = null)
        {
            moveDoneCallback = action;
            if(move != null)
            {
                OutlineOnHover.disabled = true;
                PathOnClick.pause = true;
                ExecuteMoveHelper(move);
            }
            else
            {

                ExecuteMoveHelper(null);
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
            if(team == Team.Enemy)
            {
                GetComponent<EnemyAIBasic>().ExecuteTurn(turnManager.NextTurn);
                //turnManager.NextTurn();
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
        }
    }

    
}
