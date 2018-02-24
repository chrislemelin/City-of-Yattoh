using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Common.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile2 : MonoBehaviour {

    [SerializeField]
    Image profileImage;

    [SerializeField]
    bool displayTexts = true;

    [SerializeField]
    GameObject kaProfile;

    [SerializeField]
    GameObject profilePanel;

    [SerializeField]
    private GameObject titleGameObject;
    [SerializeField]
    private GameObject textGameObject;
    private List<StatType> displayOrder = new List<StatType>() { StatType.Health, StatType.Movement, StatType.Strength, StatType.Armour, StatType.Speed };


    List<GameObject> texts = new List<GameObject>();

    /// I should really consolidate this code to profile.cs

    public void SetProfilePic(CharacterBoardEntity prim, CharacterBoardEntity ka)
    {
        profileImage.sprite = prim.ProfileImage;
        if(ka != null)
        {
            kaProfile.SetActive(true);
            kaProfile.GetComponent<Image>().sprite = ka.ProfileImageCircle;
        }
        else
        {
            kaProfile.SetActive(false);
        }
        processBoardEntity(prim);
    }

    private void processBoardEntity(BoardEntity boardEntity)
    {
        //UpdateProfilePic(boardEntity.ProfileImage);
        foreach (GameObject g in texts)
        {
            Destroy(g);
        }
        if (displayTexts)
        {
            AddTitle(boardEntity.Name);
            EvaluateStats(boardEntity);
        }
    }

    private void EvaluateStats(BoardEntity boardEntity)
    {
        foreach (StatType type in displayOrder)
        {
            Stat stat = boardEntity.Stats.GetStatInstance().GetStat(type);
            string text = boardEntity.Stats.StatToString(type);           
            AddText(text, Stats.StatTypeToTooltip(type));
        }
        AddRangeText(boardEntity);
    }

    private void AddRangeText(BoardEntity boardEntity)
    {
        string rangeText;
        if (boardEntity is CharacterBoardEntity)
        {
            if (((CharacterBoardEntity)boardEntity).Range == Skill.RANGE_ADJACENT)
            {
                AddText("Range: ADJACENT", null);
            }
            else
            {
                AddText("Range: " + ((CharacterBoardEntity)boardEntity).Range, null);
            }
        }
    }


    private void AddTitle(string text)
    {
        GameObject titleName = Instantiate(titleGameObject);
        titleName.GetComponent<TextMeshProUGUI>().text = text;
        titleName.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        titleName.transform.SetParent(profilePanel.transform, false);
        texts.Add(titleName);
    }

    private void AddText(string text, string tooltip = null)
    {
        GameObject movementStat = Instantiate(textGameObject);
        movementStat.GetComponent<TooltipSpawner>().Init(() => { return ""; }, () => { return tooltip; }, () => { return null; });
        movementStat.GetComponent<TextMeshProUGUI>().text = text;
        movementStat.transform.SetParent(profilePanel.transform, false);
        texts.Add(movementStat);
    }

}
