using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Env;

namespace Placeholdernamespace.CharacterSelection
{

    public class EnemyDropdown : MonoBehaviour
    {
        [SerializeField]
        TMP_Dropdown dropdown;

        Dictionary<string, Dictionary<Position, CharacterType>> enemyConfigurationOptions = new Dictionary<string, Dictionary<Position, CharacterType>>()
        {
            {
                "Small Group", new Dictionary<Position, CharacterType>()
                {
                    { new Position(4,0), CharacterType.EnemyBalanced },
                    { new Position(5,0), CharacterType.EnemyRanged },
                    { new Position(4,1), CharacterType.EnemySpeedy },
                }
            },
            {
                "Medium Group", new Dictionary<Position, CharacterType>()
                {
                    { new Position(9, 1), CharacterType.EnemyRanged },
                    { new Position(9, 0), CharacterType.EnemyRanged },
                    { new Position(8, 1), CharacterType.EnemyBalanced },
                    { new Position(8, 0), CharacterType.EnemyTank },
                    { new Position(9, 8), CharacterType.EnemySpeedy},
                    { new Position(9, 7), CharacterType.EnemySpeedy}
                }
            },
            {
                "Medium Group2", new Dictionary<Position, CharacterType>()
                {
                    { new Position(9, 0), CharacterType.EnemyRanged },
                    { new Position(8, 0), CharacterType.EnemyBalanced },
                    { new Position(8, 1), CharacterType.EnemyBalanced },

                    { new Position(6, 0), CharacterType.EnemyTank },
                    { new Position(6, 1), CharacterType.EnemyTank },
                }
            },
            {
                "Large Group", new Dictionary<Position, CharacterType>()
                {
                    { new Position(9, 1), CharacterType.EnemyRanged },
                    { new Position(9, 0), CharacterType.EnemyRanged },
                    { new Position(9, 2), CharacterType.EnemyRanged },
                    { new Position(9, 3), CharacterType.EnemyRanged },

                    { new Position(6, 0), CharacterType.EnemyTank },
                    { new Position(6, 1), CharacterType.EnemyTank },
                    { new Position(8, 2), CharacterType.EnemyBalanced },
                    { new Position(8, 3), CharacterType.EnemyBalanced },

                    { new Position(9, 8), CharacterType.EnemySpeedy},
                    { new Position(9, 7), CharacterType.EnemySpeedy}
                }
            }
        };

        // Use this for initialization
        void Start()
        {
            dropdown.onValueChanged.AddListener(delegate {
                DropDownSelect();
            });
            GenerateOptions();
            dropdown.value = 1;
        }

        private void DropDownSelect()
        {
            string key = dropdown.options[dropdown.value].text;
            if (enemyConfigurationOptions.ContainsKey(key))
            {
                ScenePropertyManager.Instance.Enemies = enemyConfigurationOptions[key];
            }
        }

        private void GenerateOptions()
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach(string key in enemyConfigurationOptions.Keys)
            {
                options.Add(new TMP_Dropdown.OptionData(key));
            }
            dropdown.options = options;
        }

    }

}
