using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using Placeholdernamespace.Common.Sound;

namespace Placeholdernamespace.Tests
{

    public class TestCore
    {
        
        protected IEnumerator SetUp()
        {
            var loadSceneOperation = SceneManager.LoadSceneAsync("TestBattlefield");
            loadSceneOperation.allowSceneActivation = true;


            while (!loadSceneOperation.isDone)
                yield return null;

           if(SoundManager.Instance != null)
           {
                SoundManager.Instance.setMuted(true);
           }

        }

    }
}
