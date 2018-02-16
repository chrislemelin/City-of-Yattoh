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
                if(character.second != null)
                {
                    newProfile.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    // oh god
                    Sprite kaProfile =  ScenePropertyManager.Instance.TypeToContainer[character.second.CharacterType].GetComponent<CharacterBoardEntity>().ProfileImageCircle;
                    newProfile.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = kaProfile;
                }
                profiles.Add(newProfile);
            }

        }
    }
}
