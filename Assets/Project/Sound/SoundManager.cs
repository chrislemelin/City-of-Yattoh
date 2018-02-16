using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Common.Sound
{

    public enum Soundtrack { title, battle};

    public class SoundManager : MonoBehaviour
    {

        private static SoundManager instance;
        public static SoundManager Instance
        {
            get { return instance; }
        }

        [SerializeField]
        private AudioSource musicPlayer;

        public bool muted;
        public void setMuted(bool muted)
        {
            this.muted = muted;
        }



        private List<Soundtrack> soundtrackEnum = new List<Soundtrack> {Soundtrack.title, Soundtrack.battle };
        [SerializeField]
        private List<AudioClip> soundtrackAudio;

        private Dictionary<Soundtrack, AudioClip> soundtracks = new Dictionary<Soundtrack, AudioClip>();

        private void Awake()
        {
            muted = false;
            instance = this;
            for(int a = 0; a < soundtrackEnum.Count; a++)
            {
                if(a < soundtrackAudio.Count)
                {
                    soundtracks.Add(soundtrackEnum[a], soundtrackAudio[a]);
                }
            }
            SetMusic(Soundtrack.title);
        }

        private void Update()
        {

             musicPlayer.mute = muted;

        }

        public void SetMusic(Soundtrack track)
        {
            if(soundtracks.ContainsKey(track))
            {
                musicPlayer.Stop();
                musicPlayer.loop = true;
                musicPlayer.clip = soundtracks[track];
                musicPlayer.Play();
            }
        }


    }
}
