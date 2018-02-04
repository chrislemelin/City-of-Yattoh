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

        public enum animationDirection { left, right, up, down }
        public enum animationType { idle = 0, walking = 1, attack = 2, jump = 3, damage = 4, win = 5, death = 6, skill = 7, none = 8};

        public static animationDirection GetAttackDirectionCode(Position pos, Position target)
        {
            Position direction = target - pos;

            if(Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                if( direction.x > 0)
                {
                    return animationDirection.right;
                }
                else
                {
                    return animationDirection.left;
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    return animationDirection.up;
                }
                else
                {
                    return animationDirection.down;
                }
            }
       
        }
    }

    public enum Direction { Up, Down, Left, Right };
}
