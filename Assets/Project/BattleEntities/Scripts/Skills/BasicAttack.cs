using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class BasicAttack : Skill
    {
        public BasicAttack()
        {
            title = "Basic Attack";
        }

        public override List<Tile> TileSet()
        {
            return TeamTiles(tileManager.GetAllTilesNear(boardEntity.GetTile().Position), boardEntity.Team);
        }

        public override void Action(Tile t)
        {
            battleCalculator.DoDamage((CharacterBoardEntity)boardEntity, (CharacterBoardEntity) t.BoardEntity, new Calculator.DamagePackageInternal(boardEntity.Stats.GetStatInstance().getValue(AttributeStats.StatType.Strength),Calculator.DamageType.physical));
            MonoBehaviour.print("woop an attack at "+t.Position);
        }

    }
}
