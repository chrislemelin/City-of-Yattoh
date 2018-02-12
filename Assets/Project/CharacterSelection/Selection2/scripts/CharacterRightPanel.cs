using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
            ScenePropertyManager.Instance.characters2.Clear();
            UpdateGoToBattle();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateGoToBattle()
        {
            if(ScenePropertyManager.Instance.characters2.Count == 4)
            {
                goToBattleButton.interactable = true;
                text.text = "Ready to go";
            }
            if(ScenePropertyManager.Instance.characters2.Count < 4)
            {
                goToBattleButton.interactable = false;
                text.text = "select " + (4 - ScenePropertyManager.Instance.characters2.Count) + " more to fight";
            }
            if (ScenePropertyManager.Instance.characters2.Count > 4)
            {
                goToBattleButton.interactable = false;
                text.text = "too many character selected fam";
            }
        }

        public void GoToBattle()
        {
            SceneManager.LoadScene("Battlefield");
        }

        public void ClearParty()
        {
            characterSelection2.ClearParty();
            ScenePropertyManager.Instance.characters2.Clear();
        }
    }
}
