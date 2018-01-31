using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class BuffTaunt : Buff
    {
        private HashSet<Tile> tauntTiles = new HashSet<Tile>();
        private CharacterBoardEntity tauntedBy;

        public BuffTaunt(HashSet<Tile> tauntTiles, CharacterBoardEntity tauntedBy ):base()
        {
            type = PassiveType.Debuff;
            this.tauntTiles = tauntTiles;
            this.tauntedBy = tauntedBy;
            description = "can only move adjacent to " + tauntedBy.Name;
        }

        public override HashSet<Tile> GetTauntTiles()
        {
            return tauntTiles;
        }

        public override void LeaveTile(Tile t)
        {
            if(!tauntTiles.Contains(t))
            {
                Remove();
            }
            base.LeaveTile(t);
        }


    }
}
