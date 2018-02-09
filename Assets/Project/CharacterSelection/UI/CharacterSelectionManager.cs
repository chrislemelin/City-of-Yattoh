using Placeholdernamespace.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Placeholdernamespace.CharacterSelection
{

    public class CharacterSelectionManager : MonoBehaviour {

        List<Dropdown> dropDowns = new List<Dropdown>();
        
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

            dropDowns.AddRange(GetComponentsInChildren<Dropdown>());
            foreach (Dropdown dropdown in dropDowns)
            {
                dropdown.options = GetDropDownOptions(dropdown);
                dropdown.onValueChanged.AddListener(delegate {
                    DropdownValueChanged(dropdown);
                });
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
            availibleTypes = new HashSet<CharacterType>(types);
            foreach(Dropdown dropdown in dropDowns)
            {
                string value = dropdown.options[dropdown.value].text;
                if (textToType.ContainsKey(value))
                {
                    availibleTypes.Remove(textToType[value]);
                }
            }
            foreach(Dropdown dropdown in dropDowns)
            {
                List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
                if(textToType.ContainsKey(dropdown.options[dropdown.value].text))
                {
                    options.Add(new Dropdown.OptionData(dropdown.options[dropdown.value].text));
                }              
                options.AddRange(GetDropDownOptions(dropdown));
                dropdown.options = options;
                dropdown.value = 0;
            }
            goToBattleButton.interactable = CanGoToBattle();
        }

        private bool CanGoToBattle()
        {
            bool returnBool = true;
            foreach(Dropdown dropdown in dropDowns)
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
            ScenePropertyManager.Instance.characters = new List <CharacterType>();
            foreach (Dropdown dropdown in dropDowns)
            {
                ScenePropertyManager.Instance.characters.Add(textToType[dropdown.options[dropdown.value].text]);
            }
            SceneManager.LoadScene("Battlefield");
        }
    }
}
    