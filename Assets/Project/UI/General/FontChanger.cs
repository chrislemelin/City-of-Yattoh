using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontChanger : MonoBehaviour {

    [SerializeField]
    TMP_FontAsset fontStyle;

    HashSet<GameObject> alreadyTried = new HashSet<GameObject>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(TextMeshProUGUI text in FindObjectsOfType<TextMeshProUGUI>())
        {
            if(!alreadyTried.Contains(text.gameObject))
            {
                alreadyTried.Add(gameObject);
                text.font = fontStyle;
            }
        }
	}
}
