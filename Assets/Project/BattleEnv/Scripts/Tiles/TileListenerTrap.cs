using Placeholdernamespace.Battle.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Env
{
    public class TileListenerTrap : TileListener
    {
        

        public override void TileEnter(CharacterBoardEntity chararacter, Tile t)
        {
            if (chararacter.Team == targetTeam)
            {
                //chararacter.AddPassive(new Buff)
            }
        }
    }
}
