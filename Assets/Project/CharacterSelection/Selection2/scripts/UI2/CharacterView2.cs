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
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;

namespace Placeholdernamespace.CharacterSelection
{
    public class CharacterView2 : MonoBehaviour
    {
        [SerializeField]
        GameObject displaySkill;

        [SerializeField]
        GameObject talentContainer;
        List<GameObject> talentDisplays = new List<GameObject>();

        [SerializeField]
        GameObject skillContainer;
        List<GameObject> skillDisplays = new List<GameObject>();

        //[SerializeField]
        //TextMeshProUGUI bannarMessage;

        [SerializeField]
        private CharacterSelection2 characterSelection2;

        [SerializeField]
        private Profile2 profile;

        /*
        [SerializeField]
        private Profile characterProfile;
        [SerializeField]
        private CharacterSkillView characterSkillView;
        [SerializeField]
        private CharacterSkillView kaSkillView;
        [SerializeField]
        private Profile kaProfile;
        */
        //[SerializeField]
        //private CharacterRightPanel rightPanel;
        //[SerializeField]
        //private GameObject selectDeselectButton;
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

        //private string bannarCharacterSelectMessage = "Equip Support Character, or Add to Party";
        //private string bannarKaSelectMessage = "Select skill to inherit";
        //private string bannarAddedToPartyMessage = "Choose another character";

        //private string 

        bool selectedKa = false;

        public void LockIn()
        {

            Ka ka = null;
            if (selectedKaCharacter != null)
            {
                ka = new Ka(selectedKaCharacter.GetComponent<CharContainer>());
            }
            //kaSkillView.InitKa(ka);
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
            //rightPanel.UpdateGoToBattle();
            //characterSelection2.LockIn();
        }

        public void Start()
        {
            characterSelection2.Init();
            foreach (GameObject character in ScenePropertyManager.Instance.BoardEntityCharacters.Values)
            {
                selectedCharacter = character.GetComponent<CharacterBoardEntity>();
                //DisplayCharacter(character.GetComponent<CharacterBoardEntity>(), true, false);
                break;
            }
        }

        public void DisplayCharacter(CharacterBoardEntity character, bool moveArrow = true)
        {
            selectedCharacter = character;
            //characterProfile.UpdateProfile(character);
            //characterSkillView.SetBoardEntity(character);
            DisplayKa(null);
            characterSelection2.SetSelectedCharacter(character, moveArrow);
  
        }

        private void DisplayKaHelper(Ka ka)
        {
            if (ka != null)
            {
                selectedKaCharacter = ScenePropertyManager.Instance.BoardEntityCharacters[ka.CharacterType].GetComponent<CharacterBoardEntity>();
                if(selectedKaCharacter == selectedCharacter)
                {
                    selectedKaCharacter = null;
                }
                profile.gameObject.SetActive(true);
                profile.SetProfilePic(selectedCharacter, selectedKaCharacter);
                DisplayHelper();
                //kaProfile.UpdateProfile(selectedKaCharacter);
                //kaSkillView.SetBoardEntity(selectedKaCharacter);
                //kaSkillView.SetKa(ka);
            }
            else
            {
                //kaProfile.UpdateProfile(null);
                //kaSkillView.SetBoardEntity(null);
                selectedKaCharacter = null;
                profile.gameObject.SetActive(false);
            }
            characterSelection2.SetSelectedKa(selectedKaCharacter);

        }

        private void DisplayHelper()
        {
            foreach(GameObject talent in talentDisplays)
            {
                Destroy(talent);
            }
            talentDisplays.Clear();

            foreach (GameObject skill in skillDisplays)
            {
                Destroy(skill);
            }
            skillDisplays.Clear();

            List<Passive> tempPassives = selectedCharacter.Passives;
            List<Passive> passives = new List<Passive>();
            List<Passive> talents = new List<Passive>();
            foreach (Passive passive in tempPassives)
            {
                if(passive is Talent)
                {
                    talents.Add(passive);
                }
                else
                {
                    passives.Add(passive);
                }
            }
            if(selectedKaCharacter != null)
            {
                talents.AddRange(selectedKaCharacter.GetTalents());
            }

            foreach(Passive talent in talents)
            {
                DisplayObjectHelper(talent.GetTitle(), talent.GetDescriptionHelper(), talentDisplays, talentContainer);
            }

            foreach (Skill skill in selectedCharacter.Skills)
            {
                DisplayObjectHelper(skill.GetTitle(), skill.GetDescriptionHelper(), skillDisplays, skillContainer);
            }
            foreach (Passive passive in passives)
            {
                DisplayObjectHelper(passive.GetTitleHelper(), passive.GetDescription(), skillDisplays, skillContainer);
            }

        }

        private void DisplayObjectHelper(string title, string description, List<GameObject> list, GameObject container)
        {
            GameObject display =  Instantiate(displaySkill);
            display.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
            display.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
            list.Add(display);
            display.transform.SetParent(container.transform,false);
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
            DisplayCharacter(character);
      
        }

        public void SetKa(CharacterBoardEntity character)
        {
            DisplayKa(character);
            //characterSelection2.SetSelectedKa(character);
        }

        /*
        public void SelectionDeselectButtonClick()
        {
            if (selectedKaCharacter != null)
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
        */

    }
}


