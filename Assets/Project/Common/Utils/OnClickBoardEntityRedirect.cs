using UnityEngine;
using System.Collections;

public class OnClickBoardEntityRedirect : OnClickRedirect
{

    public override IClickable GetTarget()
    {
        CharacterBoardEntity c = GetComponentInParent<CharacterBoardEntity>();
        return ((IClickable)c.Tile.GetComponentInChildren<PathOnClick>());
    }
}
