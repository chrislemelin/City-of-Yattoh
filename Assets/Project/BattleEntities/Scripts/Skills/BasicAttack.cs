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
            APCost = 1;
        }

        public override List<Tile> TileSet()
        {
            return TeamTiles(tileManager.GetAllTilesNear(boardEntity.GetTile().Position), boardEntity.Team);
        }

        public override void Action(Tile t)
        {
            int basePower = boardEntity.Stats.GetStatInstance().getValue(AttributeStats.StatType.Strength);
            Dictionary<SkillModifierType, int> baseStats = new Dictionary<SkillModifierType, int>();
            baseStats[SkillModifierType.Power] = basePower;
            Dictionary<SkillModifierType,int> effectiveStats = getSkillModifiers(baseStats);

            int effectivePower = effectiveStats[SkillModifierType.Power];

            battleCalculator.DoDamage(boardEntity, (CharacterBoardEntity) t.BoardEntity, new Calculator.DamagePackageInternal(effectivePower, Calculator.DamageType.physical));
            boardEntity.Stats.SubtractAPPoints(APCost);
            MonoBehaviour.print("woop an attack at "+t.Position);
        }

    }
}
