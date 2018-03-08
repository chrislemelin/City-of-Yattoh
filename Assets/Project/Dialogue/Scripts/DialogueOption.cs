using Placeholdernamespace.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
namespace Placeholdernamespace.Dialouge
{
    [System.Serializable]
    public class DialogueOption : IClickable
    {

        public string displayText;
        public delegate void onOptionChoose();
        public onOptionChoose onChoose;
        [System.NonSerialized]
        public DialogueFrame nextFrame;


        public void OnMouseUp()
        {
            if (onChoose != null)
                onChoose();
        }
    }
}