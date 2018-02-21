using Placeholdernamespace.Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour {

    [SerializeField]
    GameObject HelpScreen;

    public void Start()
    {
        GetComponent<OnPointerDownListener>().pressed += HelpClick;
    }

    public void HelpClick()
    {
        HelpScreen.SetActive(!HelpScreen.activeSelf);
    }
}
