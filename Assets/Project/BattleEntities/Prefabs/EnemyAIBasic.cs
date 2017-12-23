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
        private TileManager tileManager;
        private List<Action> actionQueue;
        CharacterBoardEntity characterBoardEntity;
        Skill skill;

        public void Init(TileManager tileManager, CharacterBoardEntity characterBoardEntity)
        {
            this.tileManager = tileManager;
            this.characterBoardEntity = characterBoardEntity;
        }

        public void ExecuteTurn()
        {
            List<Move> moves = characterBoardEntity.MoveSet();
            Skill skill = characterBoardEntity.BasicAttack;
            List<AiMove> aiMoves = new List<AiMove>();
            Dictionary<Move, List<BoardEntity>> moveToTargets = new Dictionary<Move, List<BoardEntity>>(); 
           
            foreach(Move m in moves)
            {
                List<Tile> tiles = skill.TheoreticalTileSet(m.destination.Position);

                List<BoardEntity> entities = new List<BoardEntity>();
                BoardEntity nearest = tileManager.NearestBoardEntity(m.destination.Position, Team.Player);
                int movementScore = tileManager.DFS(m.destination.Position, nearest.GetTile().Position, characterBoardEntity.Team).Count;
                AiMove aiMove = new AiMove(int.MaxValue, movementScore);
                aiMove.AddMoveAction(characterBoardEntity, m, DoNextAction);

                aiMoves.Add(aiMove);
                foreach(Tile t in tiles)
                {
                    if(t.BoardEntity != null)
                    {
                        aiMove = new AiMove(targetScore(t.BoardEntity), 0);
                        aiMove.AddMoveAction(characterBoardEntity, m, DoNextAction);
                        aiMove.AddAttackAction(skill, t, DoNextAction);
                        aiMoves.Add(aiMove);
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
            return boardEntity.Stats.GetMutableStat(AttributeStats.StatType.Health).Value;
        }
    }
}