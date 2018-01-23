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
        private EnemyAIBasic enemyAIBasic1;

        //private SkillSelector skillSelector;
        private Tile target = null;
        
        private Dictionary<Tile, Move> cachedMoves = new Dictionary<Tile, Move>();
        private Action moveDoneCallback;

        public override void Init(Position startingPosition, TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector, BattleCalculator battleCalculator)
        {
            base.Init(startingPosition, turnManager, tileManager, boardEntitySelector, battleCalculator);
            if(enemyAIBasic1 != null)
            {
                enemyAIBasic1.Init(tileManager, this);
            }
            basicAttack = new BasicAttack(tileManager, this, battleCalculator);
            skills.Add(basicAttack);

            passives.Add(new PassiveAreaOfInfluence(battleCalculator, this));

            List<SkillModifier> skillModifiers = new List<SkillModifier>();
            skillModifiers.Add(new SkillModifier(SkillModifierType.Power, SkillModifierApplication.Add, 1));
            passives.Add(new PassiveGeneric("Damage Buff", "Increases damage on skills by one", skillModifiers));

            foreach(Passive p in passives)
            {
                if(p is PassiveAreaOfInfluence)
                {
                    p.EnterTile(GetTile());
                }
            }
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

        public void AddBuff(Buff buff)
        {
            buff.addRemoveAction(passives.Remove);          
        }

        public List<PassiveAreaOfInfluence> GetAreaOfInfluencePassives()
        {
            List<PassiveAreaOfInfluence> passiveAreaOfInfluences = new List<PassiveAreaOfInfluence>();
            foreach (Passive p in passives)
            {
                if (p is PassiveAreaOfInfluence)
                {
                    passiveAreaOfInfluences.Add(((PassiveAreaOfInfluence)p));
                }
            }
            return passiveAreaOfInfluences;
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

    }

    
}
