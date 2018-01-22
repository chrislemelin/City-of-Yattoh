using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Battle.Entities.Skills;
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

        private List<int> displayDamages = new List<int>();

        public void ExecuteSkillDamage(CharacterBoardEntity source, Skill skill, CharacterBoardEntity target, DamagePackage damage)
        {
            SkillReport skillReport = ExecuteSkillHelper(source, skill, target, damage);            

            if (skillReport != null)
            {
                foreach (StatType type in skillReport.SourceAfter.MutableStats.Keys)
                {
                    source.Stats.SetMutableStat(type, skillReport.SourceAfter.MutableStats[type].Value);
                }
                foreach (StatType type in skillReport.TargetAfter.MutableStats.Keys)
                {
                    target.Stats.SetMutableStat(type, skillReport.TargetAfter.MutableStats[type].Value);
                }
            }


            // tell the passives what just happened
            foreach(Passive passive in source.Passives)
            {
                passive.ExecutedSkill(skillReport);
            }

            // should put this in an couroutine for later and make more better
            foreach(int displayDamageValue in displayDamages)
            {
                GameObject displayDamageObj = Instantiate(displayDamageObject);
                GameObject displayDamageObjFollow = displayDamageObj.GetComponent<FloatingText>().textMeshProUGUI.gameObject;

                displayDamageObjFollow.transform.SetParent(FindObjectOfType<Canvas>().gameObject.transform);
                displayDamageObj.transform.position = target.transform.position;
                displayDamageObjFollow.GetComponent<TextMeshProUGUI>().text = "-" + displayDamageValue.ToString();
            }
            
        }



        public SkillReport ExecuteSkillHelper(CharacterBoardEntity source, Skill skill, CharacterBoardEntity target, DamagePackage damage)
        {
            Stats sourceBefore = source.Stats.GetCopy();
            Stats sourceAfter = source.Stats.GetCopy();

            Stats targetBefore = target.Stats.GetCopy();
            Stats targetAfter = target.Stats.GetCopy();

            TakeDamageReturn usingTakeDamageReturn = TakeDamageReturn.Normal;
            List<Passive> passives = target.Passives;
            foreach(Passive passive in passives)
            {
                usingTakeDamageReturn = passive.TakeDamage(skill, damage, usingTakeDamageReturn);
            }

            displayDamages.Clear();

            switch(usingTakeDamageReturn)
            {
                case TakeDamageReturn.Normal:
                    Tuple<int,int> newTargetHealthDamage = HealthAfterDamage(source, target, damage);
                    targetAfter.SetMutableStat(StatType.Health, newTargetHealthDamage.first);
                    displayDamages.Add(newTargetHealthDamage.second);
                    return new SkillReport(sourceBefore, sourceAfter, targetBefore, targetAfter);

                case TakeDamageReturn.NoDamage:
                    return null;                    

                case TakeDamageReturn.Reflect:
                    return null;
            }

            return null;

        }

        private Tuple<int, int> HealthAfterDamage(CharacterBoardEntity source, CharacterBoardEntity target, DamagePackage damage)
        {
            int tempDamage = damage.Damage;
            if(damage.Type == DamageType.physical)
            {
                tempDamage -= target.Stats.GetNonMuttableStat(StatType.Armour).Value;
                if (tempDamage < 0)
                    tempDamage = 0;
            }
            int newHealth = target.Stats.GetMutableStat(StatType.Health).Value - tempDamage;
            if (newHealth < 0)
            {
                // dat bibba dead
                newHealth = 0;
            }
            return new Tuple<int, int> (newHealth, tempDamage);
        }

    }

    public enum TakeDamageReturn
    {
        Normal, NoDamage, Reflect
    }
}
