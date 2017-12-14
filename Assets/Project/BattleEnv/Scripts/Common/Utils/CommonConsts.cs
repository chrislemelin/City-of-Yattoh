using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Common
{
    public class CommonConsts : MonoBehaviour
    {

        private static Vector3 up = new Vector3(1, 1, 0);
        public static Vector3 Up
        {
            get { return up; }
        }

        private static Vector3 down = new Vector3(-1, -1, 0);
        public static Vector3 Down
        {
            get { return down; }
        }

        private static Vector3 right = new Vector3(1, -1, 0);
        public static Vector3 Right
        {
            get { return right; }
        }

        private static Vector3 left = new Vector3(-1, 1, 0);
        public static Vector3 Left
        {
            get { return left; }
        }
    }
    
}
