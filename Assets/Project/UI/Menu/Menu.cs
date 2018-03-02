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
        private GameObject quitButton;
        [SerializeField]
        private GameObject resetButton;
        [SerializeField]
        private GameObject soundMuteButton;
        [SerializeField]
        private GameObject soundSkipButton;

        [SerializeField]
        private string resetScene;

        // Use this for initialization
        void Start()
        {
            if(soundMuteButton != null)
                soundMuteButton.GetComponent<OnPointerDownListener>().pressed += SoundPressed;

            if(resetButton != null)
                resetButton.GetComponent<OnPointerDownListener>().pressed += ResetPressed;

            if(quitButton != null)
                quitButton.GetComponent<OnPointerDownListener>().pressed += QuitPressed;

            if(soundSkipButton != null)
                soundSkipButton.GetComponent<OnPointerDownListener>().pressed += SkipSong;

            UpdateMuteIcon();
        }

        void Update() {

        }

        private void SoundPressed()
        {
            SoundManager.Instance.muted = !SoundManager.Instance.muted;
            UpdateMuteIcon();
        }

        private void UpdateMuteIcon()
        {
            if (soundMuteButton != null)
            {
                if (SoundManager.Instance.muted)
                {
                    soundMuteButton.GetComponent<Image>().sprite = mutedMusicSprite;
                }
                else
                {
                    soundMuteButton.GetComponent<Image>().sprite = playingMusicSprite;
                }
            }
        }

        private void ResetPressed()
        {
            if(resetScene != "" || resetScene != null)
            {
                SceneManager.LoadScene(resetScene);
            }
            else
                SceneManager.LoadScene("CharacterSelection3");
        }

        private void QuitPressed()
        {
            print("quiting");
            Application.Quit();
        }

        private void SkipSong()
        {
            SoundManager.Instance.SkipSong();
        }
    }
}