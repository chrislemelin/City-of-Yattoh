using System.Collections;
using System.Collections.Generic;
using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.Skills;
using UnityEngine;
using Placeholdernamespace.Battle.Env;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public class BuffTishaProtect : Buff
    {
        private CharacterBoardEntity character;
        private float value;

        public BuffTishaProtect(CharacterBoardEntity character, float value, int stacks): base(stacks)
        {
            this.character = character;
            this.value = value;
            description = "reflect half damage when adjacent to " + character.Name;
        }

        public override TakeDamageReturn TakeDamage(Skill skill, DamagePackage package, TakeDamageReturn lastReturn)
        {
            if(IsActive())
            {
                if (lastReturn.type < TakeDamageReturnType.Reflect)
                {
                    return new TakeDamageReturn() { type = TakeDamageReturnType.Reflect, value = value };
                }
            }         
            return lastReturn;
        }

        private bool IsActive()
        {
            if (character != null)
            {
                List<Tile> tiles = tileManager.GetTilesDiag(character.Position, 1);
                foreach (Tile t in tiles)
                {
                    if (t.BoardEntity == boardEntity || t.BoardEntity == character)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
    }
}
