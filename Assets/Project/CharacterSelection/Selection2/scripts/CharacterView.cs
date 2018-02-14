using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Instances;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Battle.UI;
using Placeholdernamespace.CharacterSelection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Placeholdernamespace.CharacterSelection
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI bannarMessage;
        [SerializeField]
        private CharacterSelection2 characterSelection2;
        [SerializeField]
        private Profile characterProfile;
        [SerializeField]
        private CharacterSkillView characterSkillView;
        [SerializeField]
        private CharacterSkillView kaSkillView;
        [SerializeField]
        private Profile kaProfile;
        [SerializeField]
        private CharacterRightPanel rightPanel;
        [SerializeField]
        private GameObject selectDeselectButton;
        private CharacterBoardEntity selectedCharacter;
        public CharacterBoardEntity GetSelectedCharacter()
        {
            return selectedCharacter;
        }

        private CharacterBoardEntity selectedKaCharacter;
        public CharacterBoardEntity GetSelectedKaCharacter()
        {
            return selectedKaCharacter;
        }

        bool selectingKa = false;
        public bool SelectingKa
        {
            get { return selectingKa; }
        }

        private string bannarCharacterSelectMessage = "Equip Support Character, or Add to Party";
        private string bannarKaSelectMessage = "Select skill to inherit";
        private string bannarAddedToPartyMessage = "Choose another character";

        //private string 

        bool selectedKa = false;

        public void LockIn()
        {

            Ka ka = null;
            if (selectedKaCharacter != null)
            {
                ka = new Ka(selectedKaCharacter.GetComponent<CharContainer>());
            }
            kaSkillView.InitKa(ka);
            List<Tuple<CharacterBoardEntity, Ka>> party = new List<Tuple<CharacterBoardEntity, Ka>>(ScenePropertyManager.Instance.getCharacterParty());

            // filter out
            for (int a = 0; a < party.Count; a++)
            {
                Tuple<CharacterBoardEntity, Ka> tuple = party[a];
                if (tuple.first.CharcaterType == selectedCharacter.CharcaterType)
                {
                    party.RemoveAt(a);
                    a--;
                    continue;
                }
                if (tuple.second != null && tuple.second.CharacterType == selectedCharacter.CharcaterType)
                {
                    tuple.second = null;
                    a--;
                }
                if (selectedKaCharacter != null)
                {
                    if (tuple.first.CharcaterType == selectedKaCharacter.CharcaterType)
                    {
                        party.RemoveAt(a);
                        a--;
                    }
                    if (tuple.second != null && tuple.second.CharacterType == selectedKaCharacter.CharcaterType)
                    {
                        tuple.second = null;
                        a--;
                    }
                }
            }

            party.Add(new Tuple<CharacterBoardEntity, Ka>(selectedCharacter, ka));
            ScenePropertyManager.Instance.setCharacterParty(party);
            rightPanel.UpdateGoToBattle();
            //characterSelection2.LockIn();
        }

        public void Start()
        {
            foreach(GameObject character in ScenePropertyManager.Instance.BoardEntityCharacters.Values)
            {
                DisplayCharacter(character.GetComponent<CharacterBoardEntity>());
            }
        }

        public void DisplayCharacter(CharacterBoardEntity character, bool displayStuff = true)
        {
            selectedCharacter = character;
            characterProfile.UpdateProfile(character);
            characterSkillView.SetBoardEntity(character);
            DisplayKa(null);
            characterSelection2.SetSelectedCharacter(character);
            if(displayStuff)
            {
                bannarMessage.text = bannarCharacterSelectMessage;
            }
        }

        private void DisplayKaHelper(Ka ka)
        {
            if (ka != null)
            {
                selectedKaCharacter = ScenePropertyManager.Instance.BoardEntityCharacters[ka.CharacterType].GetComponent<CharacterBoardEntity>();
                kaProfile.UpdateProfile(selectedKaCharacter);
                kaSkillView.SetBoardEntity(selectedKaCharacter);
                kaSkillView.SetKa(ka);
                    
                selectDeselectButton.GetComponentInChildren<Text>().text = "Deselect Secondary Charcter 'Ka'";
                bannarMessage.text = bannarKaSelectMessage;
            }
            else
            {
                kaProfile.UpdateProfile(null);
                kaSkillView.SetBoardEntity(null);
                selectedKaCharacter = null;

                selectDeselectButton.GetComponentInChildren<Text>().text = "Equip Secondary Charcter 'Ka'";
                bannarMessage.text = bannarCharacterSelectMessage;
            }
            characterSelection2.SetSelectedKa(selectedKaCharacter);

        }

        public void DisplayKa(CharacterBoardEntity character)
        {
            if (character != null)
            {
                Ka ka = new Ka(character.GetComponent<CharContainer>());
                ka.AddSkill(character.Skills[0]);
                DisplayKaHelper(ka);
            }
            else
                DisplayKaHelper(null);
        }

        public void DisplayKaSet(Ka ka)
        {
            DisplayKaHelper(ka);
        }


        public void SetCharacter(CharacterBoardEntity character)
        {
            if(!selectingKa)
            {
                DisplayCharacter(character);
                DisplayKa(null);
               
            }
            else
            {
                selectingKa = false;
                DisplayKa(character);
                characterSelection2.SetSelectedKa(character);
            }
        }

       
        public void SelectionDeselectButtonClick()
        {
            if(selectedKaCharacter != null)
            {
                DisplayKa(null);
            }
            else if (!selectingKa)
            {
                selectingKa = true;
                characterSelection2.HighLightKaSelection(selectedCharacter);
            }
            else
            {
                selectingKa = false;
                characterSelection2.ClearParty((int)CharacterSelection2.ColorLocks.secondary);
                DisplayKa(null);
            }
        }

    }
}
