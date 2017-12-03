using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Common.Animator
{
    public class AnimatorUtils : MonoBehaviour
    {

        public static string Attack = "Attack";
        public static Dictionary<Direction, int> AttackDirection = new Dictionary<Direction, int>
        {
            { Direction.Up, 0 },
            { Direction.Right, 1 },
            { Direction.Down, 2 },
            { Direction.Left, 3 }
        };

        public static int GetAttackDirectionCode(Position pos, Position target)
        {
            Position direction = target - pos;

            if(Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                if( direction.x > 0)
                {
                    return AttackDirection[Direction.Right];
                }
                else
                {
                    return AttackDirection[Direction.Left];
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    return AttackDirection[Direction.Up];
                }
                else
                {
                    return AttackDirection[Direction.Down];
                }
            }
       
        }
    }

    public enum Direction { Up, Down, Left, Right };
}
