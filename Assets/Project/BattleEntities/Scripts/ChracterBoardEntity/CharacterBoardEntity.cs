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
        public float speed;

        private Tile target = null;
        private List<Tile> path;

        public override void Init(TurnManager turnManager, TileManager tileManager, TileSelectionManager tileSelectionManager, Profile profile)
        {
            base.Init(turnManager, tileManager, tileSelectionManager, profile);
        }

        public override List<Move> MoveSet()
        {
            return tileManager.DFSMoves(GetTile().Position, stats.MovementPoints);
        }

        public void ExecuteMove(Move move)
        {
            profile.UpdateProfile(null);
            if (move != null)
            {
                stats.ModifyMovementPoint(move.movementCost);
                tileSelectionManager.pause = true;
                path = move.path;
                if (path.Count > 0)
                {
                    target = path[0];
                    path.Remove(target);
                }
            }
        }

        private void checkAtTarget()
        {
            if (transform.position == target.transform.position)
            {
                tileManager.MoveBoardEntity(target.Position, this);

                if (path.Count == 0)
                {
                    target = null;
                    tileSelectionManager.pause = false;
                    tileSelectionManager.ClearGlowPath();
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
            tileSelectionManager.SelectTile(this, MoveSet(), ExecuteMove);
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
    }
}
