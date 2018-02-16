using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Placeholdernamespace.Common.Sound;
using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Battle.Entities;

namespace Placeholdernamespace.CharacterSelection
{
    public class CharacterRightPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Button goToBattleButton;

        [SerializeField]
        private CharacterSelection2 characterSelection2;

        // Use this for initialization
        void Start()
        {
            ScenePropertyManager.Instance.setCharacterParty(new List<Tuple<CharacterBoardEntity, Ka>>());
            ScenePropertyManager.Instance.updatedParty += UpdateGoToBattle;
            UpdateGoToBattle();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateGoToBattle()
        {
            if(ScenePropertyManager.Instance.GetCharacterParty().Count == 4)
            {
                goToBattleButton.interactable = true;
            }
            if(ScenePropertyManager.Instance.GetCharacterParty().Count < 4)
            {
                goToBattleButton.interactable = false;
            }
            if (ScenePropertyManager.Instance.GetCharacterParty().Count > 4)
            {
                goToBattleButton.interactable = false;
            }
        }

        public void GoToBattle()
        {
            SoundManager.Instance.SetMusic(Soundtrack.battle);
            SceneManager.LoadScene("Battlefield");
        }

        public void ClearParty()
        {
            ScenePropertyManager.Instance.setCharacterParty(new List<Tuple<CharacterBoardEntity, Ka>>());
        }
    }
}
