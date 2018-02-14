using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Instances;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Common.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Placeholdernamespace.CharacterSelection
{

    public class CharacterSelectionManager : MonoBehaviour {

        [SerializeField]
        GameObject PartySelection;

        [SerializeField]
        GameObject KaSelection;

        List<Dropdown> partyDropdowns = new List<Dropdown>();
        List<Dropdown> kaDropDowns = new List<Dropdown>();

        HashSet<CharacterType> types = new HashSet<CharacterType>() { CharacterType.PlayerAmare, CharacterType.PlayerBongani, CharacterType.PlayerDadi,
            CharacterType.PlayerJaz, CharacterType.PlayerLesidi, CharacterType.PlayerTisha };


        Dictionary<string, CharacterType> textToType = new Dictionary<string, CharacterType>();

        const string NONE_TEXT = "None";
        HashSet<CharacterType> availibleTypes;
        [SerializeField]
        private Button goToBattleButton;

	    // Use this for initialization
	    void Start () {
            availibleTypes = new HashSet<CharacterType>(types);
            foreach(CharacterType type in types)
            {
                textToType.Add(type.ToString(),type);
            }

            partyDropdowns.AddRange(PartySelection.GetComponentsInChildren<Dropdown>());
            foreach (Dropdown dropdown in partyDropdowns)
            {
                dropdown.options = GetDropDownOptions(dropdown);
                dropdown.onValueChanged.AddListener(delegate {
                    DropdownValueChanged(dropdown);
                });
            }

            kaDropDowns.AddRange(KaSelection.GetComponentsInChildren<Dropdown>());
            foreach (Dropdown dropdown in kaDropDowns)
            {
                dropdown.onValueChanged.AddListener(delegate {
                    DropdownValueChanged(dropdown);
                });
                dropdown.interactable = false;
            }

            goToBattleButton.onClick.AddListener(delegate {
                GoToBattle();
            });
            goToBattleButton.interactable = false;

        }
        private List<Dropdown.OptionData> GetDropDownOptions (Dropdown dropdown)
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            options.Add(new Dropdown.OptionData("NONE"));
            
            foreach(CharacterType type in availibleTypes)
            {
                Dropdown.OptionData option = new Dropdown.OptionData(type.ToString());    
                options.Add(option);                
            }
            return options;
        }

        private void DropdownValueChanged(Dropdown change)
        {
            UpdateOptions();
        }


        private void KaDropdownValueChanged(Dropdown change)
        {
            UpdateOptions();
        }

        private void UpdateOptions()
        {
            availibleTypes = new HashSet<CharacterType>(types);
            foreach (Dropdown dropdown in partyDropdowns)
            {
                string value = dropdown.options[dropdown.value].text;
                if (textToType.ContainsKey(value))
                {
                    availibleTypes.Remove(textToType[value]);
                }
            }
            foreach (Dropdown dropdown in kaDropDowns)
            { 
                if (dropdown.interactable)
                {
                    string value = dropdown.options[dropdown.value].text;
                    if (textToType.ContainsKey(value))
                    {
                        availibleTypes.Remove(textToType[value]);
                    }
                }
            }
            for(int a = 0; a < partyDropdowns.Count; a++)
            {
                Dropdown dropdown = partyDropdowns[a];
                List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
                if (textToType.ContainsKey(dropdown.options[dropdown.value].text))
                {
                    options.Add(new Dropdown.OptionData(dropdown.options[dropdown.value].text));
                    SetKaOptions(kaDropDowns[a]);
                    kaDropDowns[a].interactable = true;

                }
                else
                {
                    kaDropDowns[a].ClearOptions();
                    kaDropDowns[a].interactable = false;
                    
                }
                options.AddRange(GetDropDownOptions(dropdown));
                dropdown.options = options;
                dropdown.value = 0;
            }
            goToBattleButton.interactable = CanGoToBattle();
        }

        private void SetKaOptions(Dropdown dropdown)
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            if (dropdown.interactable && textToType.ContainsKey(dropdown.options[dropdown.value].text))
            {
                options.Add(new Dropdown.OptionData(dropdown.options[dropdown.value].text));
            }
            options.AddRange(GetDropDownOptions(dropdown));
            dropdown.options = options;
            dropdown.value = 0;
        }

        private bool CanGoToBattle()
        {
            bool returnBool = true;
            foreach(Dropdown dropdown in partyDropdowns)
            {
                if(!textToType.ContainsKey(dropdown.options[dropdown.value].text))
                {
                    returnBool = false;
                }
            }
            return returnBool;
        }

        private void GoToBattle()
        {
            ScenePropertyManager.Instance.setCharacterParty(new List<Tuple<CharacterBoardEntity, Ka>>());
            for (int a = 0; a < partyDropdowns.Count; a++)
            {
                Dropdown dropdown = partyDropdowns[a];
                Dropdown kaDropdown = kaDropDowns[a];
                Ka ka = null;
                if(textToType.ContainsKey(kaDropdown.options[kaDropdown.value].text))
                {
                    CharacterType kaType = textToType[kaDropdown.options[kaDropdown.value].text];
                    ka = new Ka(ScenePropertyManager.Instance.typeToContainer[kaType]);
                }
               // ScenePropertyManager.Instance.characterParty.Add( new Tuple<CharacterType, Ka>(
               //     textToType[dropdown.options[dropdown.value].text], ka));

            }
            SoundManager.Instance.SetMusic(Soundtrack.battle);
            SceneManager.LoadScene("Battlefield");
        }
    }
}
    