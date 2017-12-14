using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Placeholdernamespace.Battle.Entities.AI
{
    public class EnemyAIBasic : MonoBehaviour
    {
        private List<Action> actionQueue;
        CharacterBoardEntity boardEntity;
        Skill skill;

        public void Start()
        {
            boardEntity = GetComponent<CharacterBoardEntity>();
        }

        public void ExecuteTurn()
        {
            List<Move> moves = boardEntity.MoveSet();
            BasicAttack skill = boardEntity.BasicAttack;
            List<AiMove> aiMoves = new List<AiMove>();
            Dictionary<Move, List<BoardEntity>> moveToTargets = new Dictionary<Move, List<BoardEntity>>(); 
            foreach(Move m in moves)
            {
                List<Tile> tiles = skill.TheoreticalTileSet(m.destination.Position);

                List<BoardEntity> entities = new List<BoardEntity>();
                Action moveAction = new Action(() => boardEntity.ExecuteMove(m, DoNextAction));
                AiMove aiMove = new AiMove(int.MinValue, 0, new List<Action>() { moveAction });

                aiMoves.Add(aiMove);
                foreach(Tile t in tiles)
                {
                    if(t.BoardEntity != null)
                    {
                        Action attackAction = new Action(() => skill.Action(t, DoNextAction));                    
                        aiMove = new AiMove(targetScore(t.BoardEntity), 0, new List<Action>() { moveAction});
                        aiMove.AddAttackAction(skill, t);
                        aiMoves.Add(aiMove);
                    }
                }
                moveToTargets[m] = entities;
            }

            List<Tile> differentTiles = skill.TheoreticalTileSet(boardEntity.Position);
            foreach (Tile t in differentTiles)
            {
                if (t.BoardEntity != null)
                {
                    Action attackAction = new Action(() => skill.Action(t, DoNextAction));
                    AiMove aiMove = new AiMove(targetScore(t.BoardEntity), 0, new List<Action>() { });
                    aiMove.AddAttackAction(skill, t);
                    aiMoves.Add(aiMove);
                }
            }

            aiMoves.Sort();

            actionQueue = aiMoves[0].Actions;
            DoNextAction();

        }

    

        private void DoNextAction()
        {
            if(actionQueue.Count > 0)
            {
                Action a = actionQueue[0];
                actionQueue.RemoveAt(0);
                a();
            }
        }

        private int targetScore(BoardEntity boardEntity)
        {
            return -boardEntity.Stats.GetMutableStat(AttributeStats.StatType.Health).Value;
        }
    }
}