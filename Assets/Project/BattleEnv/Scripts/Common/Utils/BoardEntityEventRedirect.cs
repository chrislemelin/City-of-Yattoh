using UnityEngine;
using System.Collections;
using Placeholdernamespace.Common.Interfaces;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Interaction;

namespace Placeholdernamespace.Common.Utils
{
    public class BoardEntityEventRedirect : OnClickRedirect
    {

        public override IClickable GetTarget()
        {
            CharacterBoardEntity c = GetComponentInParent<CharacterBoardEntity>();
            return ((IClickable)c.GetTile().GetComponentInChildren<PathOnClick>());
        }

        public void OnMouseEnter()
        {
            CharacterBoardEntity c = GetComponentInParent<CharacterBoardEntity>();
            c.GetTile().GetComponentInChildren<OutlineOnHover>().OnMouseEnter();
            c.GetTile().GetComponentInChildren<PathOnClick>().OnMouseEnter();
        }

        public void OnMouseExit()
        {
            CharacterBoardEntity c = GetComponentInParent<CharacterBoardEntity>();
            c.GetTile().GetComponentInChildren<OutlineOnHover>().OnMouseExit();
            c.GetTile().GetComponentInChildren<PathOnClick>().OnMouseExit();
        }
    }
}