using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Common.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Placeholdernamespace.Battle.UI
{

    public class BaseSelector : MonoBehaviour
    {

        [SerializeField]
        private GameObject skillOptionButton;

        private List<Button> buttons = new List<Button>();
        private Dictionary<Button, Func<bool>> buttonToActive = new Dictionary<Button, Func<bool>>();
        protected Button cancelButton;
        private Transform targetTransform;
        private GameObject parent;

        public void Init(Transform targetTransform, GameObject parent)
        {
            this.targetTransform = targetTransform;
            this.parent = parent;
        }

        protected GameObject buildButton(string title, Action onClick, Func<String> getDescription,
            Func<string> getFlavorText, Func<bool> active, Color? color = null)
        {
            if (active != null)
                skillOptionButton.GetComponent<Button>().interactable = active();
            else
                skillOptionButton.GetComponent<Button>().interactable = true;

            GameObject skillButton = Instantiate(skillOptionButton);
            skillButton.GetComponent<TooltipSpawner>().Init(() => { return null; }, getDescription, getFlavorText);
            skillButton.GetComponentInChildren<TextMeshProUGUI>().text = title;
            skillButton.transform.SetParent(targetTransform, false);
            skillButton.GetComponent<Button>().onClick.AddListener(() => onClick());
            buttonToActive.Add(skillButton.GetComponent<Button>(), active);
            buttons.Add(skillButton.GetComponent<Button>());
            if (color != null)
            {
                skillButton.GetComponent<Image>().color = (Color)color;
            }
            skillButton.SetActive(true);
            return skillButton;
        }

        protected void ClearButtons()
        {
            foreach(Button button in new List<Button>(buttons))
            {
                buttons.Remove(button);
                buttonToActive.Remove(button);
                Destroy(button.gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(1) && cancelButton != null)
            {
                cancelButton.GetComponent<Button>().onClick.Invoke();
            }
        }

        protected void buildCancelSkillButton(Action action)
        {
            cancelButton = buildButton("Cancel", action, ReturnNull,
                ReturnNull, null, Color.white).GetComponent<Button>();
        }

        protected void buildCancelSkillButton(CharacterBoardEntity characterBoardEntity)
        {
            buildButton("End Turn", characterBoardEntity.EndMyTurn , ReturnNull,
                ReturnNull, null, Color.white).GetComponent<Button>();
        }

        protected string ReturnNull()
        {
            return null;
        }

        protected bool defaultActive()
        {
            return !PathOnClick.pause;
        }

        public void Hide()
        {
            parent.SetActive(false);
        }

        public void Show()
        {
            parent.SetActive(true);
        }

        public void SetInteractableCancelButton(bool interactable)
        {
            if(cancelButton != null)
            {
                cancelButton.interactable = interactable;
            }
        }

    }
}
