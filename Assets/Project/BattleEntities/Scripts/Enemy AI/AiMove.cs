using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Entities.AI
{
    public class AiMove : IComparable
    {

        /// <summary>
        /// is it in attack range of a weak unit?
        /// </summary>
        private int targetScore = int.MaxValue;

        /// <summary>
        /// how far away from nearest target, if it cannot attack anyone this turn
        /// </summary>
        private int movementScore = 0;

        /// <summary>
        /// how much ap will this entire turn cost
        /// </summary>
        private int apCost = 0;
        public int ApCost
        {
            get { return apCost; }
        }

        /// <summary>
        /// how many movement points will this entire turn cost?
        /// </summary>
        private int movementCost = 0;

        /// <summary>
        /// true if this move killed someone
        /// </summary>
        private bool scoredKill = false;

        private List<Action> actions = new List<Action>();
        public List<Action> Actions
        {
            get { return actions; }
        }

        public AiMove()
        {
        }

        public AiMove(int targetScore, int movementScore)
        {
            this.targetScore = targetScore;
            this.movementScore = movementScore;
        }

        public void AddAttackAction(Skill skill, Tile t, Action callBack)
        {
            SkillReport report = skill.TheoreticalAction(t);
            if(report.TargetAfter.MutableStats[AttributeStats.StatType.Health].Value == 0 )
            {
                scoredKill = true;
            }
            targetScore = report.targetAfter.GetMutableStat(AttributeStats.StatType.Health).Value;
            apCost += skill.GetAPCost();
            actions.Add(() => skill.Action(t, callBack));
        }

        public void AddMoveAction(CharacterBoardEntity boardEntity, Move move, Action<bool> callBack)
        {
            movementCost += move.movementCost;
            apCost += move.apCost;
            actions.Add(() => boardEntity.ExecuteMove(move, callBack));           
        }

        /// <summary>
        /// logic is as follows
        /// this actions kills someone
        /// otherwise try to get to the weakest target and attack
        /// will move the smallest distance possible for attacks
        /// othersise move towards closest target
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            AiMove objMove = (AiMove)obj;
            if (objMove != null)
            {
                int returnInt = scoredKill.CompareTo(objMove.scoredKill);
                if (returnInt != 0)
                {
                    return -returnInt;
                }
                returnInt = targetScore.CompareTo(objMove.targetScore);
                if (returnInt != 0)
                {
                    return returnInt;
                }

                returnInt = movementScore.CompareTo(objMove.movementScore);
                if (returnInt != 0)
                {
                    return returnInt;                   
                }

                returnInt = movementCost.CompareTo(objMove.movementCost);
                return returnInt;
            }
            else
            {
                return 0;
            }
        }
    }
}