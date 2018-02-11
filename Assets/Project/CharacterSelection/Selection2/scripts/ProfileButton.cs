using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileButton : MonoBehaviour {

	public void SetImage(Sprite image)
    {
        GetComponent<Image>().sprite = image;
    }
}
