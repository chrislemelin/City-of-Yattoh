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
    public class EnemyAIBasic : EnemyAi
    {
        private Action callBack;

        public void ExecuteTurn(Action callBack, BoardEntity ragedBy = null, HashSet<Tile> tilesAvailibleForMovement= null)
        {
            this.callBack = callBack;
            List<Move> moves = characterBoardEntity.MoveSet();
            Skill skill = characterBoardEntity.BasicAttack;
            List<AiMove> aiMoves = new List<AiMove>();
            Dictionary<Move, List<BoardEntity>> moveToTargets = new Dictionary<Move, List<BoardEntity>>();

            int counter = 0;

            foreach(Move m in moves)
            {
                List<Tile> tiles = skill.TheoreticalTileSet(m.destination.Position);

                List<BoardEntity> entities = new List<BoardEntity>();

                // raged target must be the 'nearest' boardentity
                Position targetPosition;
                if(ragedBy != null)
                {
                    targetPosition = ragedBy.GetTile().Position;
                }
                else
                {
                    BoardEntity nearest = tileManager.NearestBoardEntity(m.destination.Position, Team.Player);
                    targetPosition = nearest.GetTile().Position;
                }

                int movementScore = tileManager.DFS(m.destination.Position, targetPosition, characterBoardEntity.Team).Count;
   
                AiMove aiMove = new AiMove(int.MaxValue, movementScore);
                aiMove.AddMoveAction(characterBoardEntity, m, DoNextAction);

                aiMoves.Add(aiMove);
                foreach(Tile t in tiles)
                {
                    if(t.BoardEntity != null)
                    {
                        // must attack the raged target if there is one
                        if (ragedBy == null || ragedBy == t.BoardEntity)
                        {
                            aiMove = new AiMove(targetScore(t.BoardEntity), 0);
                            aiMove.AddMoveAction(characterBoardEntity, m, DoNextAction);
                            aiMove.AddAttackAction(skill, t, DoNextAction);
                            aiMoves.Add(aiMove);
                        }
                    }
                }
                moveToTargets[m] = entities;
            }

            // dont move, only attack
            List<Tile> differentTiles = skill.TheoreticalTileSet(characterBoardEntity.Position);
            foreach (Tile t in differentTiles)
            {
                if (t.BoardEntity != null)
                {
                    AiMove aiMove = new AiMove(targetScore(t.BoardEntity), 0);
                    aiMove.AddAttackAction(skill, t, DoNextAction);
                    aiMoves.Add(aiMove);
                }
            }

            aiMoves.RemoveAll((a) => a.ApCost > characterBoardEntity.Stats.GetMutableStat(AttributeStats.StatType.AP).Value);
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
            else
            {
                if (callBack != null)
                    callBack();
            }
        }

        private int targetScore(BoardEntity boardEntity)
        {
            return boardEntity.Stats.GetMutableStat(AttributeStats.StatType.Health).Value;
        }
    }
}