﻿using Placeholdernamespace.Battle.Entities;
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
        private CharacterBoardEntity character;
        private BoardEntity ragedBy;

        public void ExecuteTurn(CharacterBoardEntity character, Action callBack, BoardEntity ragedBy = null)
        {
            this.callBack = callBack;
            this.character = character;
            this.ragedBy = ragedBy;

            if (character.Stats.GetMutableStat(AttributeStats.StatType.AP).Value > 0)
            {
                ExecuteTurnHelper(callBack, ragedBy);
            }
            else
            {
                callBack();
            }

            
        }

        public void ExecuteTurnHelper(Action callBack, BoardEntity ragedBy = null)
        {
            this.callBack = callBack;
            List<Move> moves = characterBoardEntity.MoveSet();
            Skill skill = characterBoardEntity.BasicAttack;
            List<AiMove> aiMoves = new List<AiMove>();
            Dictionary<Move, List<BoardEntity>> moveToTargets = new Dictionary<Move, List<BoardEntity>>();
            moves.Add(new Move { destination = characterBoardEntity.GetTile() });

            foreach(Move m in moves)
            {
                List<Tile> tiles = skill.TileSetClickable(m.destination.Position);

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
                if(m.destination != characterBoardEntity.GetTile())
                {
                    aiMove.AddMoveAction(characterBoardEntity, m, DoNextAction);
                }

                aiMoves.Add(aiMove);
                foreach(Tile t in tiles)
                {
                    if(t.BoardEntity != null)
                    {
                        // must attack the raged target if there is one
                        if (ragedBy == null || ragedBy == t.BoardEntity && !((CharacterBoardEntity)t.BoardEntity).IsStealthed())
                        {
                            aiMove = new AiMove(targetScore(t.BoardEntity), 0);
                            if (m.destination != characterBoardEntity.GetTile())
                            {
                                aiMove.AddMoveAction(characterBoardEntity, m, DoNextAction);
                            }
                            aiMove.AddAttackAction(skill, t, DoNextAction);
                            aiMoves.Add(aiMove);
                        }
                    }
                }
                moveToTargets[m] = entities;
            }

            aiMoves.RemoveAll((a) => a.ApCost > characterBoardEntity.Stats.GetMutableStat(AttributeStats.StatType.AP).Value);
            aiMoves.Sort();

            actionQueue = aiMoves[0].Actions;
            DoNextAction(false);

        }

    

        private void DoNextAction(bool interupted)
        {
            if(actionQueue.Count > 0 && !interupted)
            {
                Action a = actionQueue[0];
                actionQueue.RemoveAt(0);
                a();
            }
            else
            {
                ExecuteTurn(character, callBack, ragedBy);
                //if (callBack != null)
                //   callBack();
            }
        }

        private int targetScore(BoardEntity boardEntity)
        {
            return boardEntity.Stats.GetMutableStat(AttributeStats.StatType.Health).Value;
        }
    }
}