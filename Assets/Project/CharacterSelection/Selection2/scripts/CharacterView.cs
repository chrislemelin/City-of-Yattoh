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

namespace Placeholdernamespace.CharacterSelection
{
    public class CharacterView : MonoBehaviour
    {
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
        private CharacterBoardEntity selectedKaCharacter;
        bool selectingKa = false;
        public bool SelectingKa
        {
            get { return selectingKa; }
        }

        bool selectedKa = false;

        public void LockIn()
        {
           
            Ka ka = null;
            if (selectedKaCharacter != null)
            {
                ka = new Ka(selectedKaCharacter.GetComponent<CharContainer>());
            }
            kaSkillView.InitKa(ka);

            // filter out
            for(int a = 0; a < ScenePropertyManager.Instance.characters2.Count; a++)
            {
                Tuple<CharacterType, Ka> tuple = ScenePropertyManager.Instance.characters2[a];
                if (tuple.first == selectedCharacter.CharcaterType)
                {
                    ScenePropertyManager.Instance.characters2.RemoveAt(a);
                    a--;
                }
                if (tuple.second != null && tuple.second.CharacterType == selectedCharacter.CharcaterType)
                {
                    tuple.second = null;
                    a--;
                }
                if (selectedKaCharacter != null)
                {
                    if (tuple.first == selectedKaCharacter.CharcaterType)
                    {
                        ScenePropertyManager.Instance.characters2.RemoveAt(a);
                        a--;
                    }
                    if (tuple.second != null && tuple.second.CharacterType == selectedKaCharacter.CharcaterType)
                    {
                        tuple.second = null;
                        a--;
                    }
                }
            }

            ScenePropertyManager.Instance.characters2.Add(new Tuple<CharacterType, Ka>(selectedCharacter.CharcaterType, ka));
            rightPanel.UpdateGoToBattle();
            characterSelection2.LockIn();
        }

        public void DisplayCharacter(CharacterBoardEntity character)
        {
            selectedCharacter = character;
            characterProfile.UpdateProfile(character);
            characterSkillView.SetBoardEntity(character);
            DisplayKa(null);
        }

        public void DisplayKa(CharacterBoardEntity character)
        {
            selectedKaCharacter = character;
            kaProfile.UpdateProfile(character);
            kaSkillView.SetBoardEntity(character);
            selectingKa = false;
            if(character != null)
            {
                selectedKa = true;
                selectDeselectButton.GetComponentInChildren<Text>().text = "Deselect Ka";
            }
            else
            {
                selectedKa = false;
                selectDeselectButton.GetComponentInChildren<Text>().text = "Select Ka";
            }
        }

        public void SelectionDeselectButtonClick()
        {
            if(selectedKa)
            {
                DisplayKa(null);
            }
            else if (!selectingKa)
            {
                selectingKa = true;
                characterSelection2.SelectKa(selectedCharacter);
            }
            else
            {
                characterSelection2.ClearBorders();
                DisplayKa(null);
            }
        }

    }
}
