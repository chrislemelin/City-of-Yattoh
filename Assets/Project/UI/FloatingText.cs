using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {

    public float speed;
    public float duration;
    public float startFading;
    public TextMeshProUGUI textMeshProUGUI;

    private float spawnTime;
	// Use this for initialization
	void Start () {
        spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - spawnTime > duration)
        {
            Destroy(textMeshProUGUI.gameObject);
            Destroy(gameObject);
        }
        float fade = (Time.time - spawnTime) - startFading;
        fade = fade / (duration - startFading);
        Color32 oldColor = textMeshProUGUI.color;
        if (fade > 0)
        {
            float newAlpha = ((1.0f-fade) * 255);
            textMeshProUGUI.color = new Color32(oldColor.r, oldColor.g, oldColor.b, (byte)newAlpha);
        }
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
}
