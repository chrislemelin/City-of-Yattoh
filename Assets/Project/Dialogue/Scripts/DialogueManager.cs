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
using TMPro;
using Placeholdernamespace.Common.UI;

namespace Placeholdernamespace.Dialouge
{

    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private List<Image> characterProfiles = new List<Image>();

        public TextMeshProUGUI NameText;
        public TextMeshProUGUI DialogueText;
        public GameObject DisplayPanel;
        public GameObject OptionButtonPrefab;
        public GameObject ContinueText;

        private Queue<DialogueFrame> dialogueQueue = new Queue<DialogueFrame>();
        private Animator animator;
        private List<GameObject> optionsButtons = new List<GameObject>();
        private List<DialogueCharacter> characters = new List<DialogueCharacter>();

        [SerializeField]
        private float waitTime = .05f;

        private void Start()
        {
            ContinueText.GetComponent<OnClickAction>().clickActions.Add(ContinueDialogue);
        }

        public void StartDialogue(Dialogue dialogue)
        {
            GetComponent<Animator>().SetBool("isOpen", true);
            dialogueQueue.Clear();
            foreach (DialogueFrame frame in dialogue.DialogueFrames)
            {
                dialogueQueue.Enqueue(frame);
            }
            characters = dialogue.characters;
            while(characters.Count < 4)
            {
                characters.Add(null);
            }
            SetCharacterProfilesPictures(dialogueQueue.Peek(), false);
            

        }

        private void SetCharacterProfiles(Dialogue dialogue)
        {
            for(int a = 0; a < dialogue.characters.Count && a < characterProfiles.Count; a++)
            {
                characterProfiles[a].sprite = dialogue.characters[a].GetSprite(DialogueCharacterExpression.Normal);
            }
        }

        private void SetCharacterProfilesPictures(DialogueFrame currentFrame, bool color = true)
        {
            SetUpDialogueFrame(currentFrame);
            NameText.text = GetCurrentDialogueCharacter(currentFrame).name;
            NameText.GetComponentInParent<Animator>().SetInteger("Position", (int)currentFrame.personTalkingPosition);
            for (int a = 0; a < characters.Count && a < characterProfiles.Count; a++)
            {
                if (characterProfiles[a] != null)
                {
                    if (a == (int)currentFrame.personTalkingPosition)
                    {
                        characterProfiles[a].sprite = currentFrame.personTalking.GetSprite(currentFrame.expression);

                        characterProfiles[a].GetComponent<ImageColorLerp>().lerpValue = 1.0f;


                    }
                    else
                    {
                        if (characters[a] != null)
                        {
                            characterProfiles[a].sprite = characters[a].GetSprite();
                            characterProfiles[a].GetComponent<ImageColorLerp>().lerpValue = 0.0f;
                        }
                        else
                        {
                            characterProfiles[a].GetComponent<ImageColorLerp>().primaryColor = new Color(0, 0, 0, 0);
                            characterProfiles[a].GetComponent<ImageColorLerp>().lerpValue = 0.0f;
                        }
                    }
                }
            }
        }

        public void ContinueDialogue()
        {
            
            if (dialogueQueue.Count != 0)
            {
                DialogueFrame currentFrame = dialogueQueue.Dequeue();
                SetCharacterProfilesPictures(currentFrame);
                StopAllCoroutines();
                if(currentFrame.options.Length > 0)
                {
                    ContinueText.SetActive(false);
                }
                else
                {
                    ContinueText.SetActive(true);
                }

                StartCoroutine("executeFrameCoroutines", currentFrame);
            }
            else
            {
                endDialogue();
            }
        }

        private void endDialogue()
        {
            GetComponent<Animator>().SetBool("isOpen", false);
            /*
            Animation anim = GetComponent<Animation>();
            anim["DialougeBox_Open"].speed = -1;
            anim["DialougeBox_Open"].time = anim["DialougeBox_Open"].length;
            anim.Play("DialougeBox_Open");
            */
            foreach (GameObject button in optionsButtons)
            {
                Destroy(button);
            }

        }

        private IEnumerator executeFrameCoroutines(DialogueFrame dialogueFrame)
        {
            foreach (GameObject button in optionsButtons)
            {
                Destroy(button);
            }

            yield return StartCoroutine("slowTextDisplay", dialogueFrame);
            yield return StartCoroutine("addOptionButtons", dialogueFrame);
        }

        private void SetUpDialogueFrame(DialogueFrame dialogueFrame)
        {
            if(dialogueFrame.personTalkingPosition == DialoguePosition.Null)
            {
                for(int a = 0; a < characters.Count; a++)
                {
                    if (dialogueFrame.personTalking == characters[a] && characters[a] != null)
                    {
                        dialogueFrame.personTalkingPosition = ((DialoguePosition)a);
                    }
                }
                if(dialogueFrame.personTalkingPosition == DialoguePosition.Null)
                {
                    throw new Exception("Dialogue Character not found, check the personTalking on the DialogueFrame object");
                }
            }
            if(dialogueFrame.personTalking == null)
            {
                dialogueFrame.personTalking = GetCurrentDialogueCharacter(dialogueFrame);
            }
        }

        private DialogueCharacter GetCurrentDialogueCharacter(DialogueFrame dialogueFrame)
        {
            DialogueCharacter tempCharacter = characters[(int)dialogueFrame.personTalkingPosition];
            if(tempCharacter == null)
            {
                throw new Exception("Can't find dialogue character based on DialoguePosition");
            }
            return tempCharacter;          
        }

        private IEnumerator slowTextDisplay(DialogueFrame dialogueFrame)
        {
            DialogueText.text = string.Empty;
            foreach (char newChar in dialogueFrame.dialogueText.ToCharArray())
            {
                DialogueText.text += newChar;
                yield return new WaitForSeconds(waitTime);
            }
        }

        private IEnumerator addOptionButtons(DialogueFrame dialogueFrame)
        {

            foreach (DialogueOption option in dialogueFrame.options)
            {
                GameObject button = Instantiate(OptionButtonPrefab, DisplayPanel.transform, false);
                TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
                button.GetComponent<OnClickAction>().clickActions.Add(ContinueDialogue);
                button.GetComponent<OnClickAction>().clickActions.Add(option.OnMouseUp);
                optionsButtons.Add(button);
                text.text = "\u2022<indent=3em> ";

                foreach (char newChar in option.displayText.ToCharArray())
                {
                    text.text += newChar;
                    yield return new WaitForSeconds(waitTime);
                }
            }

        }


    }
}