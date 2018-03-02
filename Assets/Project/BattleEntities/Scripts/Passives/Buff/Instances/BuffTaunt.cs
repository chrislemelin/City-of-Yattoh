using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class BuffTaunt : Buff
    {
        private CharacterBoardEntity tauntedBy;

        public BuffTaunt(CharacterBoardEntity tauntedBy ):base()
        {
            type = PassiveType.Debuff;
            this.tauntedBy = tauntedBy;
            description = "can only move adjacent to " + tauntedBy.Name;
        }

        public override HashSet<Tile> GetTauntTiles()
        {
            HashSet<Tile> set = new HashSet<Tile>();
            foreach (Tile t in tileManager.GetTilesDiag(tauntedBy.Position))
            {
                set.Add(t);
            }
            if(set.Contains(boardEntity.GetTile()))
            {
                return set;
            }
            else
            {
                Remove();
                return new HashSet<Tile>();
            }

            
        }


    }
}
