using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
using Placeholdernamespace.Common.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Placeholdernamespace.Battle.Calculator
{

    public class BattleCalculator : MonoBehaviour
    {
        [SerializeField]
        private GameObject displayDamageObject;

        private List<DamageDisplay> displayDamages = new List<DamageDisplay>();

        public SkillReport ExecuteSkillDamage(CharacterBoardEntity source, Skill skill, CharacterBoardEntity target, List<DamagePackage> damage)
        {
            SkillReport skillReport = ExecuteSkillHelper(source, skill, target, damage);
            return skillReport;
        }

        public SkillReport ExecuteSkillHealing(Skill skill, CharacterBoardEntity source, CharacterBoardEntity target, int value)
        {
            SkillReport report = new SkillReport();
            report.targetBefore = target.Stats.GetCopy();
            report.targetAfter = target.Stats.GetCopy();

            report.targetAfter.SetMutableStat(StatType.Health, report.targetAfter.GetMutableStat(StatType.Health).Value + value);
            report.targets.Add(new Tuple<Stats, Stats>(report.targetBefore, report.targetAfter));

            int healValue = report.targetAfter.GetMutableStat(StatType.Health).Value - report.targetBefore.GetMutableStat(StatType.Health).Value;

            report.TextDisplays.Add(new TextDisplay() {
                text = "+ " + healValue,
                textColor = Color.green,
                callback = (() =>  target.SetAnimation(Common.Animator.AnimatorUtils.animationType.win)),
                target = target
            });

            return report;
        }

        public void ExecuteSkillReport(SkillReport skillReport )
        {

            if (skillReport != null)
            {

                foreach(Tuple<Stats,Stats> stats in skillReport.targets)
                {
                    stats.second.BoardEntity.Stats = stats.second;
                    stats.second.UpdateStatHandler();
                }
              
                // should put this in an couroutine for later and make more better
                foreach (DamageDisplay displayDamage in skillReport.DamageDisplays)
                {
                    displayDamage.character.GetComponent<FloatingTextGenerator>().AddTextDisplay(displayDamage.GetFloatingText());
                }
                foreach (TextDisplay textDisplay in skillReport.TextDisplays)
                {
                    textDisplay.target.GetComponent<FloatingTextGenerator>().AddTextDisplay(textDisplay);
                }
            }
        }

        public SkillReport ExecuteSkillHelper(CharacterBoardEntity source, Skill skill, CharacterBoardEntity target, List<DamagePackage> damages)
        {
            SkillReport report = new SkillReport();

            report.Buffs = skill.GetBuffs();

            Stats sourceBefore = source.Stats.GetCopy();
            Stats sourceAfter = source.Stats.GetCopy();
            Stats targetBefore = target.Stats.GetCopy();
            Stats targetAfter = target.Stats.GetCopy();

            report.sourceBefore = sourceBefore;
            report.targetBefore = targetBefore;

            foreach (DamagePackage package in damages)
            {
                List<Passive> passives = target.Passives;
                TakeDamageReturn usingTakeDamageReturn = new TakeDamageReturn() { type = TakeDamageReturnType.Normal };
                foreach (Passive passive in passives)
                {
                    usingTakeDamageReturn = passive.TakeDamage(skill, package, usingTakeDamageReturn);
                }


                if (usingTakeDamageReturn.type == TakeDamageReturnType.Normal)
                {
                    targetAfter = targetBefore.GetCopy();

                    int newTargetHealthDamage = HealthAfterDamage(report, targetAfter, package);
                    targetAfter.SetMutableStat(StatType.Health, newTargetHealthDamage);
                }
                else if (usingTakeDamageReturn.type == TakeDamageReturnType.NoDamage)
                {
                    report.TextDisplays.Add(new TextDisplay() { target = target, text = "MISS" });
                }
                else if (usingTakeDamageReturn.type == TakeDamageReturnType.Mitigate)
                { 
                    targetAfter = targetBefore.GetCopy();

                    int newDamage = (int)(package.Damage * (1 - usingTakeDamageReturn.value));
                    DamagePackage newDamagePackage = new DamagePackage(newDamage, package.Type, package.Piercing);
                    int newReflectTargetHealthDamage = HealthAfterDamage(report, targetAfter, newDamagePackage);
                    targetAfter.SetMutableStat(StatType.Health, newReflectTargetHealthDamage);
                }
                else if(usingTakeDamageReturn.type == TakeDamageReturnType.Reflect)
                {
                    sourceAfter = sourceBefore.GetCopy();
                    targetAfter = targetBefore.GetCopy();

                    int newDamage = (int)(package.Damage * (1 - usingTakeDamageReturn.value));
                    int reflectDamage = (int)(package.Damage * (usingTakeDamageReturn.value));

                    DamagePackage newDamagePackage = new DamagePackage(newDamage, package.Type, package.Piercing);
                    DamagePackage reflectDamagePackage = new DamagePackage(reflectDamage, DamageType.physical);

                    int newReflectTargetHealthDamage = HealthAfterDamage(report, targetAfter, newDamagePackage);
                    int newReflectSourceHealthDamage = HealthAfterDamage(report, sourceAfter, reflectDamagePackage);

                    targetAfter.SetMutableStat(StatType.Health, newReflectTargetHealthDamage);
                    sourceAfter.SetMutableStat(StatType.Health, newReflectSourceHealthDamage);

                }              
                                      
                sourceBefore = sourceAfter;
                targetBefore = targetAfter;

            }
            report.targets.Add(new Tuple<Stats, Stats>(targetBefore, targetAfter));
            report.targets.Add(new Tuple<Stats, Stats>(source.Stats, sourceAfter));

            report.targetAfter = targetAfter;
            report.sourceAfter = sourceAfter;
            return report;



        }

        public void QuickDamage(CharacterBoardEntity target, List<DamagePackage> damages)
        {
            foreach(DamagePackage package in damages)
            {
                SkillReport report = new SkillReport();
                HealthAfterDamage(report, target.Stats, package);
                ExecuteSkillReport(report);
            }

        }

        private int HealthAfterDamage(SkillReport report,Stats targetStats, DamagePackage damage)
        {
            Stats beforeStats = targetStats.GetCopy();
            Stats afterStats = targetStats.GetCopy();
            int tempArmour = 0;
            if (damage.Type == DamageType.physical)
            {
                tempArmour = targetStats.GetNonMuttableStat(StatType.Armour).Value;
                tempArmour -= damage.Piercing;
                if (tempArmour < 0)
                {
                    tempArmour = 0;
                }
            }
            if(damage.Type == DamageType.physical || damage.Type == DamageType.pure)
            { 
                int tempDamage = damage.Damage;
                if (damage.Type == DamageType.physical)
                {
                    tempDamage -= tempArmour;
                    if (tempDamage < 0)
                        tempDamage = 0;
                }
                int oldHealth = targetStats.GetMutableStat(StatType.Health).Value;

                int newHealth = oldHealth - tempDamage;
                if (newHealth <= 0)
                {
                    // dat bibba dead
                    newHealth = 0;
                }

                DamageDisplay damageDisplay = new DamageDisplay();
                damageDisplay.value = tempDamage;
                damageDisplay.character = (CharacterBoardEntity)targetStats.BoardEntity;
                if(damage.Type == DamageType.physical)
                {
                    damageDisplay.color = Color.red;
                }
                else
                {
                    damageDisplay.color = Color.magenta;
                }
                report.DamageDisplays.Add(damageDisplay);

                afterStats.SetMutableStat(StatType.Health, newHealth);
                report.targetAfter = afterStats;
                report.targetBefore = beforeStats;                

                return newHealth;
            }
            else
                return targetStats.GetMutableStat(StatType.Health).Value;
        }

  
        /*
        private void ArmourDamage (SkillReport report, Stats targetStats, DamagePackage damage)
        {
            if (damage.Type == DamageType.armour)
            {
                int tempDamage = Mathf.Min(targetStats.GetDefaultStat(StatType.Armour).Value, damage.Damage);
                if(tempDamage > 0)
                {
                    targetStats.AddModifier(new StatModifier(StatType.Armour, StatModifierType.Add, -tempDamage));
                }
                report.DamageDisplays.Add(new DamageDisplay() { character = (CharacterBoardEntity)targetStats.BoardEntity, color = Color.blue, value = tempDamage });
            }

        }
        */
    }

    public enum TakeDamageReturnType
    {
        Normal, Mitigate, Reflect, NoDamage
    }

    public class TakeDamageReturn
    {
        public TakeDamageReturnType type;
        public float value;
    }
    

    public class DamageDisplay
    {
       //private static Dictionary<DamageType, Color>() = new Dictionary<DamageType,Color>();

        public Color color = Color.red;
        public int value;
        public CharacterBoardEntity character;

        public TextDisplay GetFloatingText()
        {
            return new TextDisplay()
            {
                textColor = color,
                text = "- " + value,
                callback = () => character.SetAnimation(Common.Animator.AnimatorUtils.animationType.damage)
            };
        }
    }
}
