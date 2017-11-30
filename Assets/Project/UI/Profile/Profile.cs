using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Placeholdernamespace.Battle.UI
{
    public class Profile : MonoBehaviour
    {
        [SerializeField]
        private GameObject titleGameObject;
        [SerializeField]
        private GameObject textGameObject;
        [SerializeField]
        private GameObject panel;

        private List<StatType> displayOrder = new List<StatType>() { StatType.Health, StatType.AP, StatType.Movement, StatType.Strength, StatType.Armour, StatType.Speed, StatType.Inteligence };

        private List<GameObject> texts = new List<GameObject>();
        private BoardEntity currentBoardEntity;

        public void Start()
        {
            gameObject.SetActive(false);

        }

        public void UpdateProfile(BoardEntity boardEntity)
        {
            if (currentBoardEntity != null)
            {
                currentBoardEntity.Stats.updateStatHandler -= RefreshProfile;
            }
            currentBoardEntity = boardEntity;
            if (boardEntity == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                currentBoardEntity.Stats.updateStatHandler += RefreshProfile;
                gameObject.SetActive(true);
                processBoardEntity(boardEntity);
            }

        }

        private void processBoardEntity(BoardEntity boardEntity)
        {
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
                Stat stat = boardEntity.stats.GetStatInstance().GetStat(type);
                string text = boardEntity.stats.StatToString(type);
                AddText(text);
            }
        }

        private void AddTitle(string text)
        {
            GameObject titleName = Instantiate(titleGameObject);   
            titleName.GetComponent<Text>().text = text;
            titleName.transform.SetParent(panel.transform);
            texts.Add(titleName);
        }

        private void AddText(string text)
        {
            GameObject movementStat = Instantiate(textGameObject);
            movementStat.GetComponent<Text>().text = text;
            movementStat.transform.SetParent(panel.transform);
            texts.Add(movementStat);
        }

    }
}