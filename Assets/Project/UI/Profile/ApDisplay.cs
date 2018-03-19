using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Placeholdernamespace.Battle.UI
{

    public class ApDisplay : MonoBehaviour
    {

        [SerializeField]
        BoardEntitySelector boardEntitySelector;

        [SerializeField]
        GameObject ApPip;

        [SerializeField]
        GameObject apDisplayPanel;

        private List<GameObject> ApPips = new List<GameObject>();

        public void DisplayAp(BoardEntity character, Stats previewStats = null)
        {
            int grey = 0;
            int ap = character.Stats.GetDefaultStat(StatType.AP).Value;
            if (previewStats != null)
            {
                grey = character.Stats.GetDefaultStat(StatType.AP).Value
                    - previewStats.GetDefaultStat(StatType.AP).Value;
                ap = previewStats.GetDefaultStat(StatType.AP).Value;
                if (grey < 0)
                    grey = 0;
            }

            grey = 8 - ap;
            ClearPips();

            for (int a = 0; a < ap; a++)
            {
                GameObject instance = Instantiate(ApPip);
                instance.GetComponent<Image>().color = Color.red;
                ApPips.Add(instance);
                instance.transform.SetParent(apDisplayPanel.transform, false);
            }

            for (int a = 0; a < grey; a++)
            {
                GameObject instance = Instantiate(ApPip);
                instance.GetComponent<Image>().color = Color.grey;
                ApPips.Add(instance);
                instance.transform.SetParent(apDisplayPanel.transform, false);
            }

           
            
        }

        private void ClearPips()
        {
            foreach(GameObject pip in ApPips)
            {
                Destroy(pip);
            }
            ApPips.Clear();
        }

    }
}
