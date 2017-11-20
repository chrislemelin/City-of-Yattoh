using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.Managers;
using Placeholdernamespace.Battle.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Placeholdernamespace.Battle.Entities
{
    public class CharacterBoardEntity : BoardEntity
    {
        [SerializeField]
        private float speed = 5;
        private SkillSelector skillSelector;
        private Tile target = null;
        private List<Tile> path;
        private Dictionary<Tile, Move> cachedMoves = new Dictionary<Tile, Move>();

        public void Init(TurnManager turnManager, TileManager tileManager, BoardEntitySelector boardEntitySelector)
        {
            base.Init(turnManager, tileManager, boardEntitySelector);
        }

        public override List<Move> MoveSet()
        {
            return tileManager.DFSMoves(GetTile().Position, stats.MovementPoints);
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

        public override void OnSelect()
        {
            base.OnSelect();
            //boardEntitySelector.setSelectedBoardEntity(this);
            /*
            base.OnSelect();
            skillSelector.SetSkills(skills);
            List<Move> moves = MoveSet();
            cachedMoves.Clear();
            foreach(Move m in moves)
            {
                cachedMoves.Add(m.destination, m);
            }

            tileSelectionManager.SelectTile(this, moves, ExecuteMove, null, null);
            */
        }

        public void ExecuteMove(Move move)
        {
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

        public override void MyTurn()
        {
            stats.NewTurn();
        }

        private void ExecuteMoveHelper(Move move)
        {
            if (move != null)
            {
                stats.ModifyMovementPoint(move.movementCost);
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
