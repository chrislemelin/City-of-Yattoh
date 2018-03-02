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
        GameObject supportTalentContainter;

        [SerializeField]
        GameObject talentContainer;
        List<GameObject> talentDisplays = new List<GameObject>();

        [SerializeField]
        GameObject skillContainer;

        [SerializeField]
        GameObject helpButton;

        [SerializeField]
        GameObject passiveContainer;
        List<GameObject> skillDisplays = new List<GameObject>();

        [SerializeField]
        private CharacterSelection2 characterSelection2;

        [SerializeField]
        private List<Profile2> profiles;

        [SerializeField]
        private KaSkillSelect2 kaSkillSelect;

        [SerializeField]
        private Button addToParty;

        [SerializeField]
        private TextMeshProUGUI addToPartyText;

        [SerializeField]
        GameObject profileDisplay;

        [SerializeField]
        GameObject profileHidden;

        [SerializeField]
        KaSkillSelect2 kaSkillSelect2;

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

   

        //private string bannarCharacterSelectMessage = "Equip Support Character, or Add to Party";
        //private string bannarKaSelectMessage = "Select skill to inherit";
        //private string bannarAddedToPartyMessage = "Choose another character";

        //private string 


        public void LockIn()
        {
            Ka ka = kaSkillSelect2.Ka;
            //kaSkillView.InitKa(ka);
            List<Tuple<CharacterBoardEntity, Ka>> party = new List<Tuple<CharacterBoardEntity, Ka>>(ScenePropertyManager.Instance.GetCharacterParty());

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
                    //a--;
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
                        //a--;
                    }
                }
            }

            party.Add(new Tuple<CharacterBoardEntity, Ka>(selectedCharacter, ka));
            if (party.Count > 4)
            {
                addToPartyText.text = "Can only add 4 to party";
            }
            else
            {
                Clear();
                ScenePropertyManager.Instance.SetCharacterParty(party);
                if (party.Count == 4)
                {
                    addToPartyText.text = "Ready to go";
                }
                else
                {
                    addToPartyText.text = "Need " + (4 - party.Count) + " more for a full party";
                }
            }
        }

        public void Clear()
        {
            characterSelection2.Clear();
            profileDisplay.SetActive(false);
            helpButton.SetActive(false);
            profileHidden.SetActive(true);
            addToParty.interactable = false;
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
            profileDisplay.SetActive(false);
            helpButton.SetActive(false);
            profileHidden.SetActive(true);
            addToParty.interactable = false;
            DisplayKa(null);
            characterSelection2.SetSelectedCharacter(character, moveArrow);
  
        }

        private void DisplayKaHelper(Ka ka)
        {
            if (ka != null)
            {
                selectedKaCharacter = ScenePropertyManager.Instance.BoardEntityCharacters[ka.CharacterType].GetComponent<CharacterBoardEntity>();
                characterSelection2.SetSelectedKa(selectedKaCharacter);
                if (selectedKaCharacter == selectedCharacter)
                {
                    selectedKaCharacter = null;
                }
                foreach (Profile2 profile in profiles)
                {
                    profile.gameObject.SetActive(true);
                    profile.SetProfilePic(selectedCharacter, selectedKaCharacter);
                }       
                DisplayHelper();
                kaSkillSelect.Init(selectedKaCharacter, EnableAddToParty);
                profileDisplay.SetActive(true);
                helpButton.SetActive(true);
                profileHidden.SetActive(false);
                if (selectedKaCharacter != null)
                {
                    addToParty.interactable = false;
                    addToPartyText.text = "Select a support skill";
                }
                else
                {
                    EnableAddToParty();
                }
            }
            else
            {
                selectedKaCharacter = null;
                foreach (Profile2 profile in profiles)
                {
                    profile.gameObject.SetActive(false);
                }
            }

            //characterSelection2.SetSelectedKa(selectedKaCharacter);

        }

    

        public void EnableAddToParty()
        {
            addToPartyText.text = "Ready to add to party";
            addToParty.interactable = true;
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
            List<Passive> talentTriggers = new List<Passive>();
            List<Passive> supportTalents = new List<Passive>();
            foreach (Passive passive in tempPassives)
            {
                if (passive is Talent)
                {
                    talents.Add(passive);
                }
                else if (passive is TalentTrigger)
                {
                    talentTriggers.Add(passive);
                }
                else
                {
                    passives.Add(passive);
                }
            }
            if (selectedKaCharacter != null)
            {
                supportTalents.AddRange(selectedKaCharacter.GetTalents());
            }

            foreach (Passive talent in talents)
            {
                DisplayObjectHelper(talent.GetTitle(), talent.GetDescriptionHelper(), talentDisplays, talentContainer);
            }
            foreach (Passive talent in talentTriggers)
            {
                DisplayObjectHelper(talent.GetTitle(), talent.GetDescription(), talentDisplays, passiveContainer);
            }

            foreach (Passive passive in passives)
            {
                DisplayObjectHelper(passive.GetTitleHelper(), passive.GetDescription(), skillDisplays, passiveContainer);
            }
            foreach (Skill skill in selectedCharacter.Skills)
            {
                DisplayObjectHelper(skill.GetTitle(), skill.GetDescriptionHelper(), skillDisplays, skillContainer);
            }
            foreach (Passive talent in supportTalents)
            {
                GameObject temp = DisplayObjectHelper(talent.GetTitle(), talent.GetDescriptionHelper(), talentDisplays, supportTalentContainter);
                temp.transform.SetAsFirstSibling();
            }

        }

        private GameObject DisplayObjectHelper(string title, string description, List<GameObject> list, GameObject container)
        {
            GameObject display =  Instantiate(displaySkill);
            display.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
            display.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
            list.Add(display);
            display.transform.SetParent(container.transform,false);
            return display;
        }


        public void DisplayKa(CharacterBoardEntity character)
        {
            if (character != null)
            {
                Ka ka = new Ka(character.GetComponent<CharContainer>());
                DisplayKaHelper(ka);
            }
            else
                DisplayKaHelper(null);
        }

        public void SetDisplayKa(Ka ka)
        {
            DisplayKaHelper(ka);
        }


    }
}


