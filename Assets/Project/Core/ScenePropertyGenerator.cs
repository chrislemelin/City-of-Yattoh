using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Common
{
    public class ScenePropertyGenerator : MonoBehaviour
    {

        [SerializeField]
        GameObject SceneProperty;

        // Use this for initialization
        void Awake()
        {
            if(ScenePropertyManager.Instance == null)
            {
                Instantiate(SceneProperty).GetComponent<ScenePropertyManager>().Init();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
