using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testScript : MonoBehaviour
{
    RectTransform rt;
    TextMeshProUGUI txt;

    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>(); // Acessing the RectTransform 
        txt = gameObject.GetComponent<TextMeshProUGUI>(); // Accessing the text component
    }

    void Update()
    {
        //rt.sizeDelta = new Vector2(rt.rect.width, txt.renderedHeight); // Setting the height to equal the height of text
        Set_Height(gameObject, txt.renderedHeight);
        Set_Width(gameObject, txt.renderedWidth);
    }


    public void Set_Height(GameObject gameObject, float height)
    {
        if (gameObject != null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
            }
        }
    }

    public void Set_Width(GameObject gameObject, float width)
    {
        if (gameObject != null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
            }
        }

    }
}
