using Placeholdernamespace.Battle.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.CharacterSelection {

   

    public class CharacterSelection2 :  MonoBehaviour{

        [SerializeField]
        private CharacterView characterView;

        [SerializeField]
        private GameObject charButton;

        private CharacterBoardEntity initalCharacterBoardEntity;
        private CharacterBoardEntity selectedCharacter;
        private CharacterBoardEntity selectedKaCharacter;
        private List<GameObject> charButtons = new List<GameObject>();
        private Dictionary<CharacterBoardEntity, GameObject> charToButton = new Dictionary<CharacterBoardEntity, GameObject>();

        // Use this for initialization
        void Start() {
                     
            foreach(GameObject character in ScenePropertyManager.Instance.BoardEntityCharacters.Values)
            {
                if(initalCharacterBoardEntity == null)
                {
                    initalCharacterBoardEntity = character.GetComponent<CharacterBoardEntity>();
                }
                charButtons.Add(MakeButton(character.GetComponent<CharacterBoardEntity>()));
            }
            SetUpInitalSetup();

        }

        private void SetUpInitalSetup()
        {
            SelectCharacter(initalCharacterBoardEntity);
            characterView.DisplayKa(null);
        }

        public void SelectKa(CharacterBoardEntity character)
        {
            foreach(GameObject button in charButtons)
            {
                if (charToButton[selectedCharacter] != button)
                {
                    button.GetComponent<ColorEffectManager>().TurnOn(this, Color.red);
                }
            }
        }

        private GameObject MakeButton(CharacterBoardEntity character)
        {
            GameObject buttonInstance = Instantiate(charButton);
            charToButton.Add(character, buttonInstance);
            buttonInstance.GetComponentInChildren<ProfileButton>().SetImage(character.ProfileImage);
            buttonInstance.GetComponent<OnClickAction>().clickAction = () => SelectCharacter(character);
            buttonInstance.transform.SetParent(transform, false);
            return buttonInstance;
            
        }

        public void ClearBorders(CharacterBoardEntity exclude)
        {
            ClearBorders(new List<CharacterBoardEntity>() { exclude });
        }

        public void ClearBorders(List<CharacterBoardEntity> exclude = null)
        {
            List<GameObject> excludeButtons = new List<GameObject>();
            if(exclude != null)
            {
                foreach(CharacterBoardEntity character in exclude)
                {
                    excludeButtons.Add(charToButton[character]);
                }
            }
            foreach (GameObject button in charButtons)
            {
                if (!excludeButtons.Contains(button))
                {
                    button.GetComponent<ColorEffectManager>().TurnOff(this);
                }

            }
        }

        private void SelectCharacter(CharacterBoardEntity character)
        {
            if (characterView.SelectingKa)
            {
                if(character != selectedCharacter)
                {
                    characterView.DisplayKa(character);
                    ClearBorders(new List<CharacterBoardEntity>() { selectedCharacter, character });
                    selectedKaCharacter = character;
                }
            }
            else
            {
                if(selectedCharacter != null)
                {
                    charToButton[selectedCharacter].GetComponent<ColorEffectManager>().TurnOff(this);
                }
                characterView.DisplayCharacter(character);
                if (selectedKaCharacter != null)
                {
                    charToButton[selectedKaCharacter].GetComponent<ColorEffectManager>().TurnOff(this);
                }
                charToButton[character].GetComponent<ColorEffectManager>().TurnOn(this, Color.yellow);
                selectedCharacter = character;
                
                selectedKaCharacter = null;

            }
        }

        public void ClearParty()
        {
            foreach(GameObject button in charToButton.Values)
            {
                button.GetComponent<ColorEffectManager>().TurnOff(this);
                button.GetComponent<ColorEffectManager>().TurnOff(characterView);
            }
        }

        

        public void LockIn()
        {
            if(selectedCharacter!= null)
            {
                charToButton[selectedCharacter].GetComponent<ColorEffectManager>().TurnOff(this);
                charToButton[selectedCharacter].GetComponent<ColorEffectManager>().TurnOff(characterView);
                charToButton[selectedCharacter].GetComponent<ColorEffectManager>().TurnOn(characterView, Color.green);
            }
            if (selectedKaCharacter != null)
            {
                charToButton[selectedKaCharacter].GetComponent<ColorEffectManager>().TurnOff(this);
                charToButton[selectedKaCharacter].GetComponent<ColorEffectManager>().TurnOff(characterView);
                charToButton[selectedKaCharacter].GetComponent<ColorEffectManager>().TurnOn(characterView, Color.grey);
            }
        }



    }
}
