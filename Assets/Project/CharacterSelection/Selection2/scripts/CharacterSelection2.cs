using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Kas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.CharacterSelection {

   

    public class CharacterSelection2 :  MonoBehaviour{

        public enum ColorLocks { primary, secondary }

        [SerializeField]
        private CharacterView characterView;

        [SerializeField]
        private GameObject charButton;

        //private CharacterBoardEntity initalCharacterBoardEntity;
        //private CharacterBoardEntity selectedCharacter;
        //private CharacterBoardEntity selectedKaCharacter;
        private List<GameObject> charButtons = new List<GameObject>();
        private Dictionary<CharacterBoardEntity, GameObject> charToButton = new Dictionary<CharacterBoardEntity, GameObject>();

        // Use this for initialization
        void Awake() {
                     
            foreach(GameObject character in ScenePropertyManager.Instance.BoardEntityCharacters.Values)
            {
                
                charButtons.Add(MakeButton(character.GetComponent<CharacterBoardEntity>()));
            }
            SetUpInitalSetup();

        }

        private void SetUpInitalSetup()
        {
            //SelectCharacter(initalCharacterBoardEntity);
            //characterView.DisplayKa(null);
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

 



        private void SelectCharacter(CharacterBoardEntity character)
        {
            characterView.SetCharacter(character);
        }
        
        public void SetSelectedCharacter(CharacterBoardEntity character)
        {
            ClearParty((int)ColorLocks.primary);
            if(character != null)
                charToButton[character].GetComponent<ColorEffectManager>().TurnOn((int)ColorLocks.primary, Color.yellow);
        }

        public void HighLightKaSelection(CharacterBoardEntity character)
        {
            foreach(KeyValuePair<CharacterBoardEntity, GameObject> thing in charToButton)
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
                charToButton[character].GetComponent<ColorEffectManager>().TurnOn((int)ColorLocks.secondary, Color.red);
            

        }

        public void ClearParty(int locky)
        {
            foreach(GameObject button in charToButton.Values)
            {
                button.GetComponent<ColorEffectManager>().TurnOff(locky) ;
                //button.GetComponent<ColorEffectManager>().TurnOff(characterView);
            }
            SetUpInitalSetup();
        }

        public void ClearEverything()
        {
            ClearParty((int)ColorLocks.primary);
            ClearParty((int)ColorLocks.secondary);
            ClearParty((int)ColorLocks.primary);
            ClearParty((int)ColorLocks.primary);

        }


    }
}
