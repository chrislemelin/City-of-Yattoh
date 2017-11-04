using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueFrame {
	
	[TextArea(3,10)]
	public string dialogueText;
	public string name;
	public Image portrait;
	public DialogueOption[] options;

}
