using Placeholdernamespace.Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.CharacterSelection
{
    public class PartyProfile : MonoBehaviour
    {
        [SerializeField]
        private OnPointerDownListener exit;
        public OnPointerDownListener Exit
        {
            get { return exit; }
        }

        [SerializeField]
        private OnPointerDownListener profile;
        public OnPointerDownListener Profile
        {
            get { return profile; }
        }

        // Use this for initialization
        void Start()
        {
            
        }
   

        // Update is called once per frame
        void Update()
        {

        }
    }
}
