using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Placeholdernamespace.Common.UI
{
    public class HighlightTextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Color defaultColor;

        [SerializeField]
        private Color highlightColor;

        [SerializeField]
        private float speed = .5f;

        private float currentAplha = 0;
        private TextMeshProUGUI text;
        private bool entered = false;

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnDisable()
        {
            entered = false;
        }

        private void Update()
        {
            if (entered)
            {
                currentAplha += Time.deltaTime * speed;
                if (currentAplha > 1)
                {
                    currentAplha = 1;
                }
            }

            else
            {
                currentAplha -= Time.deltaTime * speed;
                if (currentAplha < 0)
                {
                    currentAplha = 0;
                }
            }

            //Color color = text.color;//s.GetColor(ShaderUtilities.ID_UnderlayColor);
            //text.outlineColor = new Color(color.r, color.b, color.g, currentAplha);
            text.faceColor = Color.Lerp(defaultColor, highlightColor, currentAplha);
            //text.faceColor = new Color(1* currentAplha, 1* currentAplha, 1* currentAplha);
            //text.materialForRendering.SetColor(ShaderUtilities.ID_UnderlayColor, new Color(color.r, color.g, color.b, currentAplha));         

        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            entered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            entered = false;
        }


    }
}
