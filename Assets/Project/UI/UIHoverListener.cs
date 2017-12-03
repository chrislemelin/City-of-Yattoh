
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
namespace Placeholdernamespace.Battle.UI
{
    public class UIHoverListener : MonoBehaviour
    {        
        public static bool isUIOverride { get; private set; }

        void Update()
        {
            // It will turn true if hovering any UI Elements
            isUIOverride = EventSystem.current.IsPointerOverGameObject();
        }
    }
}
