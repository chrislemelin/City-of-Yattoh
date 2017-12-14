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
        private int targetScore;

        /// <summary>
        /// is it getting closer to a weak unit?
        /// </summary>
        private int movementScore;

        /// <summary>
        /// true if this move killed someone
        /// </summary>
        private bool scoredKill = false;

        private List<Action> actions;
        public List<Action> Actions
        {
            get { return actions; }
        }

        public AiMove(int targetScore, int movementScore, List<Action> actions)
        {
            this.targetScore = targetScore;
            this.movementScore = movementScore;
            this.actions = actions;
        }

        public void AddAttackAction(BasicAttack skill, Tile t)
        {
            SkillReport report = skill.TheoreticalAction(t);
            if(report.TargetAfter[AttributeStats.StatType.Health].Value == 0 )
            {
                scoredKill = true;
            }
            actions.Add(() => skill.Action(t));
        }

        /// <summary>
        /// logic is as follows
        /// this actions kills someone
        /// otherwise try to get to the weakest target and attack
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
                    return -returnInt;
                }
                returnInt = movementScore.CompareTo(objMove.movementScore);
                return -returnInt;
            }
            else
            {
                return 0;
            }
        }
    }
}