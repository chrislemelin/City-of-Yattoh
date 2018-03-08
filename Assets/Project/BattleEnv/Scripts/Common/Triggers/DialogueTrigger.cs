using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Dialouge
{

    public class DialogueTrigger : MonoBehaviour
    {
        public DialogueManager dialogueManager;
        public Dialogue dialogue;

        public void TriggerDialouge()
        {
            dialogueManager.gameObject.SetActive(true);
            dialogueManager.StartDialogue(dialogue);
        }
    }
}
