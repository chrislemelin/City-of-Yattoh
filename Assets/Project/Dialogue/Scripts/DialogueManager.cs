using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class DialogueManager: MonoBehaviour {

	public Text NameText;
	public Text DialogueText;
    public GameObject DisplayPanel;
    public GameObject OptionButtonPrefab;

	private Queue<DialogueFrame> dialogueQueue = new Queue<DialogueFrame>();
	private Animator animator;
    private List<GameObject> optionsButtons = new List<GameObject>();
	private DialogueFrame currentFrame;

    private const string CLOSE_STATE = "Base Layer.DialougeBox_Close";

	public void Start()
	{
		animator = GetComponent<Animator> ();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		animator.SetBool ("isOpen", true);
		dialogueQueue.Clear ();
		foreach (DialogueFrame frame in dialogue.DialogueFrames)
		{
			dialogueQueue.Enqueue (frame);
		}
		NameText.text = dialogue.name;
		ContinueDialogue();
	}

	public void ContinueDialogue()
	{
		if (dialogueQueue.Count != 0) {
			currentFrame = dialogueQueue.Dequeue ();
			StopAllCoroutines ();
     
            StartCoroutine ("executeFrameCoroutines");
		} 
		else {
			endDialogue ();
		}
	}

	private void endDialogue()
	{
		animator.SetBool ("isOpen", false);
	}

    private IEnumerator executeFrameCoroutines()
    {
        foreach (GameObject button in optionsButtons)
        {
            Destroy(button);
        }
        yield return StartCoroutine(waitForLoading());
        yield return StartCoroutine(slowTextDisplay());
        yield return StartCoroutine(addOptionButtons());
    }

    private IEnumerator waitForLoading()
    {
        bool isLoading = animator.GetCurrentAnimatorStateInfo(0).IsName(CLOSE_STATE);
        while (isLoading)
        {
            isLoading = animator.GetCurrentAnimatorStateInfo(0).IsName(CLOSE_STATE);
            yield return new WaitForEndOfFrame();
        }
    }

	private IEnumerator slowTextDisplay()
	{
       
		DialogueText.text = string.Empty;
		foreach (char newChar in currentFrame.dialogueText.ToCharArray()) {
			DialogueText.text += newChar;
			yield return null;
		}
	}

    private IEnumerator addOptionButtons()
    {
        
        foreach (DialogueOption option in currentFrame.options)
        {
            GameObject button = Instantiate(OptionButtonPrefab);
            button.transform.parent = DisplayPanel.transform;
            Text text = button.GetComponentInChildren<Text>();
            button.GetComponent<Button>().onClick.AddListener(ContinueDialogue);
            button.GetComponent<Button>().onClick.AddListener(option.OnMouseDown);
            optionsButtons.Add(button);

            foreach (char newChar in option.displayText.ToCharArray())
            {
                text.text += newChar;
                yield return null;
            }
        }

    }

	
}
