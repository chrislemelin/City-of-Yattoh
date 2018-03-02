using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Placeholdernamespace.Title
{

    public class TitlePlayButton : MonoBehaviour
    {
        public void GoToCharacterSelection()
        {

            SceneManager.LoadScene("CharacterSelection3");
        }
 
    }
}
