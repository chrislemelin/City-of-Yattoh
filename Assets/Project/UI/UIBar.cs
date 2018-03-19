using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Placeholdernamespace.Battle.UI
{
    public class UIBar : MonoBehaviour
    {
        Color fullColor = Color.green;
        Color halfColor = Color.yellow;
        Color quarterColor = Color.red;


        [SerializeField]
        private GameObject fill;

        private BoardEntity boardEntity;

        private float value;

        public void SetValue()
        {
            float newHealth = (float)boardEntity.Stats.GetMutableStat(StatType.Health).Value / (float)boardEntity.Stats.GetStatInstance().getValue(StatType.Health);
            this.value = newHealth;
            fill.transform.localScale = new Vector3(this.value, fill.transform.localScale.y, fill.transform.localScale.z);
            if (newHealth >= .5f)
            {
                fill.GetComponent<Image>().color = fullColor;
            }
            else if (newHealth >= .25f)
            {
                fill.GetComponent<Image>().color = halfColor;
            }
            else
            {
                fill.GetComponent<Image>().color = quarterColor;
            }
        
        }

        public void Init(BoardEntity boardEntity, bool follow = true)
        {
            this.boardEntity = boardEntity;
            if(follow)
            {
                transform.position = new Vector3(100000, 100000);
                GetComponent<UIFollow>().target = boardEntity.gameObject;

            }
            boardEntity.Stats.updateStatHandler += SetValue;
            SetValue();
        }

        public void StayTheSameSize()
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        public void Start()
        {
            // we dont want this to scale with the screen size :(
        }

        private void OnDestroy()
        {
            if(boardEntity!=null)
                boardEntity.Stats.updateStatHandler -= SetValue;
        }

    }
}