using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Common.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Placeholdernamespace.Battle.UI
{
    public class Profile : MonoBehaviour
    {
        [SerializeField]
        private GameObject profilePic;
        [SerializeField]
        private GameObject titleGameObject;
        [SerializeField]
        private GameObject textGameObject;
        [SerializeField]
        private GameObject profilePanel;

        [SerializeField]
        private GameObject passivePanel;
        [SerializeField]
        private GameObject passiveGameObject;

        [SerializeField]
        private bool showPassives = true;

        [SerializeField]
        private ApDisplay apDisplay;

        [SerializeField]
        private Color posColor = Color.green;
        [SerializeField]
        private Color negColor = Color.red;

        private List<StatType> displayOrder = new List<StatType>() { StatType.Health, StatType.AP, StatType.Movement, StatType.Strength, StatType.Armour, StatType.Speed };

        private List<GameObject> texts = new List<GameObject>();
        private List<GameObject> passives = new List<GameObject>();
        private BoardEntity currentBoardEntity;

        public void Start()
        {
            //gameObject.SetActive(false);
            //GetComponent<VerticalLayoutGroup>()
        }


        public void UpdateProfile(BoardEntity boardEntity, Stats previewStats = null, SkillReport skillReport = null)
        {
            if (currentBoardEntity != null)
            {
                currentBoardEntity.updateStatHandler -= RefreshProfile;
            }
            currentBoardEntity = boardEntity;
            if (boardEntity == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                currentBoardEntity.updateStatHandler += RefreshProfile;
                gameObject.SetActive(true);
                processBoardEntity(boardEntity, previewStats, skillReport);
            }

        }

        public void PreviewMove(BoardEntity boardEntity, Move move)
        {
            Stats stats = boardEntity.Stats.GetCopy();
            stats.SubtractAPPoints(move.apCost);
            stats.SetMutableStat(StatType.Movement, move.movementPointsAfterMove);
            processBoardEntity(boardEntity, stats);
        }

        private void processBoardEntity(BoardEntity boardEntity, Stats previewStats = null, SkillReport skillreport = null)
        {
            if(previewStats != null)
            {
                apDisplay.DisplayAp(boardEntity, previewStats);
            }
            else
            {
                apDisplay.DisplayAp(boardEntity);
            }
            UpdateProfilePic(boardEntity.ProfileImage);
            foreach (GameObject g in texts)
            {
                Destroy(g);
            }
            foreach (GameObject g in passives)
            {
                Destroy(g);
            }
            AddTitle(boardEntity.Name);
            EvaluateStats(boardEntity, previewStats, skillreport);
            if (showPassives)
            {
                if (boardEntity is CharacterBoardEntity)
                {
                    AddPassives(((CharacterBoardEntity)boardEntity).Passives);
                }
            }
        }

        private void RefreshProfile(object sender)
        {
            processBoardEntity(currentBoardEntity);
        }

        private void AddPassives(List<Passive> passives)
        {
            foreach(Passive passive in passives)
            {
                GameObject passiveObject = Instantiate(passiveGameObject);
                passiveObject.GetComponent<TooltipSpawner>().Init(passive.GetTitle, passive.GetDescription, ()=> { return null; });
                passiveObject.transform.SetParent(passivePanel.transform);
                this.passives.Add(passiveObject);
            }
            if(passives.Count == 0)
            {
                passivePanel.gameObject.SetActive(false);
            }
            else
            {
                passivePanel.gameObject.SetActive(true);
            }

        }

        private void EvaluateStats(BoardEntity boardEntity, Stats previewStats = null, SkillReport skillReport = null)
        {
            foreach(StatType type in displayOrder)
            {
                Stat stat = boardEntity.Stats.GetStatInstance().GetStat(type);
                string text = boardEntity.Stats.StatToString(type);
                if(previewStats != null)
                {
                    Color? col = GetStatChangeColor(boardEntity, previewStats, type);
                    if(col != null)
                    {
                        text += ColorText((Color)col, " -> " + previewStats.StatValueString(type));
                    }
                }
                if(skillReport != null)
                {
                    List<StatModifier> mods = new List<StatModifier>();
                    foreach(Buff buff in skillReport.Buffs)
                    {
                        mods.AddRange(buff.GetStatModifiers());
                    }
                    int value =  skillReport.targetAfter.GetDefaultStat(type, mods).Value;
                    skillReport.targetAfter.modifiers = mods;

                    Color? col = GetStatChangeColor(boardEntity, skillReport.targetAfter, type);
                    if (col != null)
                    {
                        text += ColorText((Color)col, " -> " + skillReport.targetAfter.StatValueString(type));
                    }
                    skillReport.targetAfter.modifiers = new List<StatModifier>();
                }
                AddText(text, Stats.StatTypeToTooltip(type));
            }
            AddRangeText(boardEntity);
        }
        
        private void AddRangeText(BoardEntity boardEntity)
        {
            string rangeText;
            if(boardEntity is CharacterBoardEntity)
            {
                if(((CharacterBoardEntity)boardEntity).Range == Skill.RANGE_ADJACENT)
                {
                    AddText("Range: ADJACENT",null);
                }
                else
                {
                    AddText("Range: " + ((CharacterBoardEntity)boardEntity).Range, null);
                }
            }
        }


        private Color? GetStatChangeColor(BoardEntity boardEntity, Stats previewStats, StatType type)
        {
            int before = boardEntity.Stats.GetDefaultStat(type).Value;
            int after = previewStats.GetDefaultStat(type).Value;
            if(before > after)
            {
                return negColor;
            }
            if(after > before)
            {
                return posColor;
            }
            return null;      

        }


        private Color? GetStatChangeColor(BoardEntity boardEntity, int perviewValue, StatType type)
        {
            int before = boardEntity.Stats.GetDefaultStat(type).Value;
            int after = perviewValue;
            if (before > after)
            {
                return negColor;
            }
            if (after > before)
            {
                return posColor;
            }
            return null;

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
            movementStat.GetComponent<TooltipSpawner>().Init(() => { return ""; }, () => { return tooltip; }, ()=> { return null; });
            movementStat.GetComponent<TextMeshProUGUI>().text = text;
            movementStat.transform.SetParent(profilePanel.transform, false);
            texts.Add(movementStat);
        }

        private void UpdateProfilePic(Sprite sprite)
        {
            profilePic.GetComponent<Image>().sprite = sprite;
            profilePic.SetActive(sprite != null);
        }

        private string ColorText(Color col, string text)
        {
            return "<#" + ColorUtility.ToHtmlStringRGB(col) + ">" + text + "</color>";
        }
    }
}