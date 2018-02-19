using Placeholdernamespace.Battle.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Env
{
    public delegate bool EnterTileAction(Tile tile, CharacterBoardEntity character);

    public class TileListener: MonoBehaviour
    {

        private Func<TileListener, bool> removeAction;
        private EnterTileAction enterAction;

        public void Init(Func<TileListener, bool> remove)
        {
            removeAction = remove;
        }

        private void Remove()
        {       
            if(removeAction != null)
            {
                removeAction(this);
            }
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        public void SetEnterAction(EnterTileAction action)
        {
            enterAction = action;
        }

        public virtual void TileEnter(CharacterBoardEntity character, Tile t) {
            if (enterAction != null)
            {
                if(enterAction(t, character))
                {
                    Remove();
                }
            }
        }
        public virtual void TileLeave(CharacterBoardEntity character, Tile t) { }

    }
}

