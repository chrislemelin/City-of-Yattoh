using UnityEngine;
using System.Collections;
using Placeholdernamespace.Common.Interfaces;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Interaction;

namespace Placeholdernamespace.Common.Utils
{
    public class OnClickBoardEntityRedirect : OnClickRedirect
    {

        public override IClickable GetTarget()
        {
            CharacterBoardEntity c = GetComponentInParent<CharacterBoardEntity>();
            return ((IClickable)c.GetTile().GetComponentInChildren<PathOnClick>());
        }
    }
}