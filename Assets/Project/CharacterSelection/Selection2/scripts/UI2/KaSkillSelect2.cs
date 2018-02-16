using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Instances;
using Placeholdernamespace.Battle.Entities.Kas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Battle.Entities.Passives;
using System;
using UnityEngine.EventSystems;

public class KaSkillSelect2 : MonoBehaviour {

    [SerializeField]
    Color greyOut;

    [SerializeField]
    GameObject parent;

    [SerializeField]
    GameObject displaySkill;

    [SerializeField]
    TMP_Dropdown dropDown;

    [SerializeField]
    GameObject skillContainer;

    [SerializeField]
    GameObject lastDisplay = null;




    private Ka ka;
    public Ka Ka
    {
        get { return ka; }
    }
    private CharContainer charContainer;

    Dictionary<TMP_Dropdown.OptionData, Passive> optionToPassive = new Dictionary<TMP_Dropdown.OptionData, Passive>();
    Dictionary<TMP_Dropdown.OptionData, Skill> optionToSkill = new Dictionary<TMP_Dropdown.OptionData, Skill>();

    private TMP_Dropdown.OptionData selectOption;
    private Action selected;

    private void Start()
    {
        dropDown.onValueChanged.AddListener(delegate {
            DropDownSelect();
        });
    }

    public void Init(CharacterBoardEntity character, Action selected)
    {
        this.dropDown.value = 0;
        this.selected = selected;
        ClearDisplay();
        if(character != null)
        {
            parent.GetComponent<Image>().color = Color.white;
            gameObject.SetActive(true);

            ka = new Ka(character.GetComponent<CharContainer>());          
            GenerateOptions(character);
        }
        else
        {
            parent.GetComponent<Image>().color = greyOut;
            gameObject.SetActive(false);
            ka = null;
        }
    }


    private void DropDownSelect()
    {
        TMP_Dropdown.OptionData option = dropDown.options[dropDown.value];
        if (option != selectOption)
        {
            selected();
            if (dropDown.options.Contains(selectOption))
            {
                dropDown.options.Remove(selectOption);
            }

            ka = new Ka(charContainer);
            if(optionToPassive.ContainsKey(option))
            {
                ka.AddPassive(optionToPassive[option]);
                DisplayObjectHelper(optionToPassive[option].GetTitleHelper(), optionToPassive[option].GetDescriptionHelper(), skillContainer);
            }
            if (optionToSkill.ContainsKey(option))
            {
                ka.AddSkill(optionToSkill[option]);
                DisplayObjectHelper(optionToSkill[option].GetTitle(), optionToSkill[option].GetDescriptionHelper(), skillContainer);

            }
        }
    }


    private void DisplayObjectHelper(string title, string description, GameObject container)
    {
        ClearDisplay();
        lastDisplay = Instantiate(displaySkill);
        lastDisplay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
        lastDisplay.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
        lastDisplay.transform.SetParent(container.transform, false);
    }



    private void ClearDisplay()
    {
        Destroy(lastDisplay);
        lastDisplay = null;
    }

    private void GenerateOptions(CharacterBoardEntity character, bool first = true)
    {
        charContainer = character.GetComponent<CharContainer>();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        selectOption = new TMP_Dropdown.OptionData("Select a skill");
        options.Add(selectOption);

        TMP_Dropdown.OptionData option;
        foreach(Skill skill in character.Skills)
        {
            option = new TMP_Dropdown.OptionData(skill.GetTitle());
            optionToSkill.Add(option, skill);
            options.Add(option);
        }
        foreach (Passive p in character.Passives)
        {
            if(!(p is Talent))
            {
                option = new TMP_Dropdown.OptionData(p.GetTitleHelper());
                optionToPassive.Add(option, p);
                options.Add(option);
            }
        }
        dropDown.options = options;
    }

    

}
