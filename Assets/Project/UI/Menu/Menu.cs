using Placeholdernamespace.Common.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Placeholdernamespace.Common.UI
{
    public class Menu : MonoBehaviour {


        [SerializeField]
        private Sprite mutedMusicSprite;
        [SerializeField]
        private Sprite playingMusicSprite;

        [SerializeField]
        private GameObject soundButton;

        [SerializeField]
        private GameObject resetButton;

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            soundButton.GetComponent<OnPointerDownListener>().pressed += SoundPressed;
            resetButton.GetComponent<OnPointerDownListener>().pressed += ResetPressed;
            UpdateMuteIcon();
        }

        // Update is called once per frame
        void Update() {

        }

        private void SoundPressed()
        {
            SoundManager.Instance.muted = !SoundManager.Instance.muted;
            UpdateMuteIcon();
        }

        private void UpdateMuteIcon()
        {
            if (SoundManager.Instance.muted)
            {
                soundButton.GetComponent<Image>().sprite = mutedMusicSprite;
            }
            else
            {
                soundButton.GetComponent<Image>().sprite = playingMusicSprite;
            }
        }

        private void ResetPressed()
        {
            resetButton.SetActive(false);
            SceneManager.LoadScene("CharacterSelection3");
        }
    }
}