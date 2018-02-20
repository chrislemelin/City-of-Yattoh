using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Placeholdernamespace.CharacterSelection {

   

    public class CharacterSelection2 :  MonoBehaviour{

        [SerializeField]
        Sprite noneSprite;

        [SerializeField]
        Color greyOutColor;
   

        public enum ColorLocks { primary, secondary }
        [SerializeField]
        bool version2;

        [SerializeField]
        private CharacterView2 characterView;

        [SerializeField]
        private GameObject charButton;

        [SerializeField]
        private GameObject characterSelectionContainer;

        [SerializeField]
        private GameObject kaSelectionContainer;

        [SerializeField]
        private GameObject arrow;

        private Dictionary<CharacterBoardEntity, GameObject> charToButton = new Dictionary<CharacterBoardEntity, GameObject>();
        private Dictionary<CharacterBoardEntity, GameObject> kaToButton = new Dictionary<CharacterBoardEntity, GameObject>();


        // Use this for initialization
        public void  Init() {
            ScenePropertyManager.Instance.SetCharacterParty(new List<Tuple<CharacterBoardEntity, Ka>>());

            foreach (GameObject character in ScenePropertyManager.Instance.BoardEntityCharacters.Values)
            {
                MakeButton(character.GetComponent<CharacterBoardEntity>());
            }
            ScenePropertyManager.Instance.updatedParty += GreyOutUsedCharacters;
            //RemakeKaButtons();
        }

        private void OnDestroy()
        {
            ScenePropertyManager.Instance.updatedParty -= GreyOutUsedCharacters;
        }

        public void Clear()
        {
            ClearEverything();
            arrow.SetActive(false);
        }

        private void RemakeKaButtons()
        {

            ClearKaButtons();
            CharacterBoardEntity selectedCharacter = characterView.GetSelectedCharacter();
            HashSet<CharacterBoardEntity> usedChars = ScenePropertyManager.Instance.GetUsedCharacters();
            foreach (GameObject character in ScenePropertyManager.Instance.BoardEntityCharacters.Values)
            {
                bool blank = selectedCharacter == character.GetComponent<CharacterBoardEntity>();
                MakeKaButton(character.GetComponent<CharacterBoardEntity>(), blank);
            }
            GreyOutUsedCharacters();
        }

        public void ClearKaButtons()
        {
            foreach(GameObject button in kaToButton.Values)             
            {
                Destroy(button);
            }
            kaToButton.Clear();
        }

        private GameObject MakeButton(CharacterBoardEntity character)
        {
            GameObject buttonInstance = Instantiate(charButton);
            charToButton.Add(character, buttonInstance);
            buttonInstance.GetComponentInChildren<ProfileButton>().SetImage(character.ProfileImage);
            buttonInstance.GetComponent<OnPointerDownListener>().pressed += () => SelectCharacter(character);
            buttonInstance.transform.SetParent(characterSelectionContainer.transform, false);
            return buttonInstance;
            
        }

        public void GreyOutUsedCharacters()
        {
            HashSet<CharacterBoardEntity> usedChars = ScenePropertyManager.Instance.GetUsedCharacters();
           
           foreach(CharacterBoardEntity character in charToButton.Keys)
           {
                if(!usedChars.Contains(character))
                {
                    charToButton[character].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    charToButton[character].GetComponent<OnPointerDownListener>().active = true;
                }
                else
                {
                    charToButton[character].transform.GetChild(0).GetComponent<Image>().color = greyOutColor;
                    charToButton[character].GetComponent<OnPointerDownListener>().active = false;

                }
            }

            foreach (CharacterBoardEntity character in kaToButton.Keys)
            {
                if (!usedChars.Contains(character))
                {
                    kaToButton[character].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    kaToButton[character].GetComponent<OnPointerDownListener>().active = true;
                }
                else
                {
                    kaToButton[character].transform.GetChild(0).GetComponent<Image>().color = greyOutColor;
                    kaToButton[character].GetComponent<OnPointerDownListener>().active = false;
                }
            }

        }

        private GameObject MakeKaButton(CharacterBoardEntity character, bool blank = false)
        {
            GameObject buttonInstance = Instantiate(charButton);
            kaToButton.Add(character, buttonInstance);
            buttonInstance.GetComponent<OnPointerDownListener>().pressed += () => SelectKa(character);
            if (blank)
            {
                buttonInstance.GetComponentInChildren<ProfileButton>().SetImage(noneSprite);
                buttonInstance.GetComponent<OnPointerDownListener>().OnPointerDown(null);
            }
            else
            {
                buttonInstance.GetComponentInChildren<ProfileButton>().SetImage(character.ProfileImage);
            }

            buttonInstance.transform.SetParent(kaSelectionContainer.transform, false);
            return buttonInstance;
        }

        private void SelectCharacter(CharacterBoardEntity character)
        {
            characterView.DisplayCharacter(character);
            
        }

        private void SelectKa(CharacterBoardEntity character)
        {
            characterView.DisplayKa(character);
        }

        public void SetSelectedCharacter(CharacterBoardEntity character, bool moveArrow = true)
        {
            RemakeKaButtons();
            ClearParty((int)ColorLocks.primary);
            if (character != null)
            {
                charToButton[character].GetComponent<ColorEffectManager>().TurnOn((int)ColorLocks.primary, Color.yellow);
                if(moveArrow && arrow != null)
                {
                    arrow.SetActive(true);
                    //arrow.transform.position = new Vector3(110, charToButton[character].GetComponent<RectTransform>().position.y, 0);
                    //ugh this is gross
                    arrow.GetComponent<RectTransform>().anchoredPosition = new Vector3 (100,
                        charToButton[character].GetComponent<RectTransform>().localPosition.y *
                        charToButton[character].GetComponent<RectTransform>().localScale.y  , 0);
                }

            }
        }

        public void HighLightKaSelection(CharacterBoardEntity character)
        {
            foreach(KeyValuePair<CharacterBoardEntity, GameObject> thing in kaToButton)
            {
                if(thing.Key != character)
                {
                    thing.Value.GetComponent<ColorEffectManager>().TurnOn((int)ColorLocks.secondary, Color.red);
                }
            }
        }

        public void SetSelectedKa(CharacterBoardEntity character)
        {
            ClearParty((int)ColorLocks.secondary);
            if (character != null)
                kaToButton[character].GetComponent<ColorEffectManager>().TurnOn((int)ColorLocks.secondary, Color.red);

        }

        public void ClearParty(int locky)
        {
            foreach(GameObject button in charToButton.Values)
            {
                button.GetComponent<ColorEffectManager>().TurnOff(locky) ;
                //button.GetComponent<ColorEffectManager>().TurnOff(characterView);
            }
            foreach(GameObject button in kaToButton.Values)
            {
                button.GetComponent<ColorEffectManager>().TurnOff(locky) ;
                //button.GetComponent<ColorEffectManager>().TurnOff(characterView);
            }
        }

        public void ClearEverything()
        {
            ClearKaButtons();
            ClearParty((int)ColorLocks.primary);
            ClearParty((int)ColorLocks.secondary);

        }


    }
}
