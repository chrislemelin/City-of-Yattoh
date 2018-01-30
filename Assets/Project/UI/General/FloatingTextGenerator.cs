using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Placeholdernamespace.Battle.Entities;

namespace Placeholdernamespace.Common.UI

{
    public class FloatingTextGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject textDisplayObject;

        private List<TextDisplay> textDisplays = new List<TextDisplay>();
        private float lastDisplay;
        private float waitTime = 1;

        // Use this for initialization
        void Start()
        {
            lastDisplay = Time.time;
        }
        
        // Update is called once per frame
        void Update()
        {
            if(textDisplays.Count > 0 && (Time.time - lastDisplay > waitTime))
            {
                DisplayNext();
            }
        }

        private void DisplayNext()
        {
            lastDisplay = Time.time;
            TextDisplay text = textDisplays[0];
            textDisplays.RemoveAt(0);
            

            GameObject displayDamageObj = Instantiate(textDisplayObject);
            GameObject displayDamageObjFollow = displayDamageObj.GetComponent<FloatingText>().textMeshProUGUI.gameObject;

            displayDamageObj.transform.position = gameObject.transform.position;
            displayDamageObjFollow.transform.SetParent(FindObjectOfType<Canvas>().gameObject.transform);
            displayDamageObjFollow.GetComponent<TextMeshProUGUI>().text = text.text;
            displayDamageObjFollow.GetComponent<TextMeshProUGUI>().color = text.textColor;
            if(text.callback != null)
            {
                text.callback();
            }

           
        }

        public void AddTextDisplay(TextDisplay textDisplay)
        {
            textDisplays.Add(textDisplay);
        }
    }

    public class TextDisplay
    {
        public string text;
        public Color textColor;
        public Action callback;
        public CharacterBoardEntity target;
    }
}
