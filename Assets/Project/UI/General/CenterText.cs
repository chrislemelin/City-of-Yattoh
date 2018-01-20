using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class CenterText : MonoBehaviour {

    private static CenterText instance;
    public static CenterText Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Image background;

    [SerializeField]
    private float startFading;

    private float? spawnTime;
    private float duration;
    private Action callback;

	// Use this for initialization
	void Start () {
        if(instance == null)
        {
            instance = this;
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if (spawnTime != null && Time.time - spawnTime > duration)
        {
            text.text = "";
            text.enabled = false;
            background.enabled = false;
            spawnTime = null;
            callback();
        }
        if (spawnTime != null)
        {
            float fade = (Time.time - (float)spawnTime) - startFading;
            fade = fade / (duration - startFading);
            Color32 oldColor = text.color;
            Color32 oldBGColor = background.color;
            if (fade > 0)
            {
                float newAlpha = ((1.0f - fade) * 255);
                Color newColor = new Color32(oldColor.r, oldColor.b, oldColor.g, (byte)newAlpha);
                background.color = new Color32(oldBGColor.r, oldBGColor.b, oldBGColor.g,(byte)newAlpha);
                text.color = newColor;
            }
        }
    }

   public void DisplayMessage(string message, Action callback, int duration = 4, int startFading = 3)
    {
        text.text = message;
        spawnTime = Time.time;
        this.duration = duration;
        this.startFading = startFading;

        text.enabled = true;
        background.enabled = true;

        Color32 oldColor = text.color;
        text.color = new Color32(oldColor.r, oldColor.b, oldColor.g, 255);

        Color32 backgroundColor = background.color;
        background.color = new Color32(backgroundColor.r, backgroundColor.b, backgroundColor.g, 255);
        this.callback = callback;

    }

}
