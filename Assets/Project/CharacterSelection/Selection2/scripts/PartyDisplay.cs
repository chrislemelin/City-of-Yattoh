using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Kas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Placeholdernamespace.CharacterSelection
{

    public class PartyDisplay : MonoBehaviour
    {
        [SerializeField]
        private CharacterView2 characterView;

        [SerializeField]
        private GameObject partyDisplayGrid;

        [SerializeField]
        private GameObject profile;
        List<GameObject> profiles = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            ScenePropertyManager.Instance.updatedParty += UpdateProfile;
        }

        private void OnDestroy()
        {
            ScenePropertyManager.Instance.updatedParty -= UpdateProfile;
        }

        private void UpdateProfile()
        {
            foreach(GameObject profile in profiles)
            {
                Destroy(profile);
            }
            profiles.Clear();

            foreach(Tuple<CharacterBoardEntity,Ka> character in ScenePropertyManager.Instance.GetCharacterParty())
            {
                GameObject newProfile = Instantiate(profile);
                newProfile.transform.GetChild(0).GetComponent<Image>().sprite = character.first.ProfileImage;
                newProfile.transform.SetParent(partyDisplayGrid.transform, false);
                newProfile.GetComponent<PartyProfile>().Exit.pressed += () => { RemoveCharacter(character.first); };
                newProfile.GetComponent<PartyProfile>().Profile.pressed += () => { DisplayCharacter(character.first, character.second); };
                if (character.second != null)
                {
                    newProfile.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    // oh god
                    Sprite kaProfile =  ScenePropertyManager.Instance.TypeToContainer[character.second.CharacterType].GetComponent<CharacterBoardEntity>().ProfileImageCircle;
                    newProfile.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = kaProfile;
                }
                profiles.Add(newProfile);
            }

        }

        private void DisplayCharacter(CharacterBoardEntity character, Ka ka)
        {
            characterView.DisplayCharacter(character);
            characterView.DisplayKa(ScenePropertyManager.Instance.TypeToBE[ka.CharacterType]);
        }

        private void RemoveCharacter(CharacterBoardEntity character)
        {
            List<Tuple<CharacterBoardEntity, Ka>> party = ScenePropertyManager.Instance.GetCharacterParty();
            for(int a = 0; a < party.Count; a++)
            {
                if(party[a].first == character)
                {
                    party.RemoveAt(a);
                    break;
                }
            }
            ScenePropertyManager.Instance.SetCharacterParty(party);
        }
    }
}
