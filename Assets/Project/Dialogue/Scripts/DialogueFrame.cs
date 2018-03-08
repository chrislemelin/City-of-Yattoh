using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Placeholdernamespace.Dialouge
{

    public enum DialoguePosition {Null = -1, LeftPrimary = 0, LeftSeconday = 1, RightSecondary = 2, RightPrimary = 3 }

    [System.Serializable]
    public class DialogueFrame
    {

        [TextArea(3, 10)]
        public string dialogueText;
        public DialogueOption[] options;

        // should be used when multiple of the same character talking 
        public DialoguePosition personTalkingPosition;
        // should be used in all other cases
        public DialogueCharacter personTalking;

        public DialogueCharacterExpression expression = DialogueCharacterExpression.Normal;
        public DialogueFrame nextFrame;

        public List<DialoguePosition> leaving;

        public List<DialoguePosition> enteringPosition;
        public List<DialogueCharacter> enteringCharacter;

    }
}