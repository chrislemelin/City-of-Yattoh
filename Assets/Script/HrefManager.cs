/// Credit playemgames 
/// Sourced from - http://forum.unity3d.com/threads/sprite-icons-with-text-e-g-emoticons.265927/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;

public class HrefManager : MonoBehaviour
{
    
	public List<HyperLinkDetails> hyperLinkDetails = new List<HyperLinkDetails> ();

	void Start() {
		gameObject.GetComponent<TextPic>().onHrefClick.AddListener (OnHrefClick);
	}

    void OnDisable()
    {
		gameObject.GetComponent<TextPic>().onHrefClick.RemoveListener (OnHrefClick);
    }

    private void OnHrefClick(string hrefName)
    {
        Debug.Log("Click on the " + hrefName);
		for(int i = 0; i < hyperLinkDetails.Count; i++) {
			if (hyperLinkDetails [i].hyperlinkName == hrefName) {
				UiManager.Instance.hrefDetailRect.SetActive (true);
				UiManager.Instance.closeButton.SetActive(true);
				UiManager.Instance.hrefDetailText.text = hyperLinkDetails [i].hyperlinkDescription;
				break;
			}
		}
    }
}

[System.Serializable]
public class HyperLinkDetails {
	public string hyperlinkName;
	public string hyperlinkDescription;
}
