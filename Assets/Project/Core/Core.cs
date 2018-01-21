using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Placeholdernamespace.Common.UI;

namespace Placeholdernamespace
{
    public class Core : MonoBehaviour
    {
        static public Core instance; //the instance of our class that will do the work

        void Awake()
        { //called when an instance awakes in the game
            instance = this; //set our static reference to our newly initialized instance
        }


        private static IEnumerator CallbackDelayHelper(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }

        static public void CallbackDelay(float seconds, Action callback)
        {
            instance.StartCoroutine(CallbackDelayHelper(seconds,callback)); //this will launch the coroutine on our instance
        }

    }
    
}
