using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Dialouge
{
    [System.Serializable]
    public class Dialogue {

        public List<DialogueCharacter> characters; 
        public string name;
        public bool linear;
        public DialogueFrame[] DialogueFrames;
    }
}