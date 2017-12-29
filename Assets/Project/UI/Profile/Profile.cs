using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
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
        private GameObject panel;


        public GameObject obj;
        private List<StatType> displayOrder = new List<StatType>() { StatType.Health, StatType.AP, StatType.Movement, StatType.Strength, StatType.Armour, StatType.Speed, StatType.Inteligence };

        private List<GameObject> texts = new List<GameObject>();
        private BoardEntity currentBoardEntity;

        public void Start()
        {
            gameObject.SetActive(false);
            //GetComponent<VerticalLayoutGroup>()
        }

        public void UpdateProfile(BoardEntity boardEntity)
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
                processBoardEntity(boardEntity);
            }

        }

        private void processBoardEntity(BoardEntity boardEntity)
        {
            UpdateProfilePic(boardEntity.ProfileImage);
            foreach (GameObject g in texts)
            {
                Destroy(g);
            }
            AddTitle(boardEntity.Name);
            EvaluateStats(boardEntity);
        }

        private void RefreshProfile(object sender)
        {
            processBoardEntity(currentBoardEntity);
        }

        private void EvaluateStats(BoardEntity boardEntity)
        {
            foreach(StatType type in displayOrder)
            {
                Stat stat = boardEntity.Stats.GetStatInstance().GetStat(type);
                string text = boardEntity.Stats.StatToString(type);
                AddText(text);
            }
        }

        private void AddTitle(string text)
        {
            GameObject titleName = Instantiate(titleGameObject);   
            titleName.GetComponent<TextMeshProUGUI>().text = text;
            titleName.transform.SetParent(panel.transform, false);
            texts.Add(titleName);
        }

        private void AddText(string text)
        {
            GameObject movementStat = Instantiate(textGameObject);
            movementStat.GetComponent<TextMeshProUGUI>().text = text;
            movementStat.transform.SetParent(panel.transform, false);
            texts.Add(movementStat);
        }

        private void UpdateProfilePic(Sprite sprite)
        {
            profilePic.GetComponent<Image>().sprite = sprite;
            profilePic.SetActive(sprite != null);
        }

    }
}