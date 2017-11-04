using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour {

	public static UiManager Instance;

	public GameObject hrefDetailRect;
	public Text hrefDetailText;


	public Text hrefText;

	public GameObject closeButton;

	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	//<a href=police>police</a>
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCloseButtonClicked () {
		hrefDetailRect.SetActive(false);
		closeButton.SetActive(false);
	}
}
