using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Placeholdernamespace.Common.UI;
using Placeholdernamespace.Battle.Entities;

namespace Placeholdernamespace
{
    public class Core : MonoBehaviour
    {
        static private Core instance; //the instance of our class that will do the work
        static public Core Instance
        {
            get { return instance; }
        }

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

        public static List<CharacterBoardEntity> convert(List<BoardEntity> things)
        {
            List<CharacterBoardEntity> returnList = new List<CharacterBoardEntity>();
            foreach(BoardEntity b in things)
            {
                if(b is CharacterBoardEntity)
                {
                    returnList.Add((CharacterBoardEntity)b);
                }
            }
            return returnList;

        }

        public CharacterBoardEntity convert(BoardEntity boardEntity)
        {
            if (boardEntity is CharacterBoardEntity)
            {
                return (CharacterBoardEntity)boardEntity;
            }
            return null;
        }
        
    }
    
}
