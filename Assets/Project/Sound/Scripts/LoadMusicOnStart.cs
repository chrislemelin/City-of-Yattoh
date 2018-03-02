using Placeholdernamespace.Common.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMusicOnStart : MonoBehaviour {

    [SerializeField]
    public Soundtrack track;

	// Use this for initialization
	void Start () {
        SoundManager.Instance.SetMusic(track);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
