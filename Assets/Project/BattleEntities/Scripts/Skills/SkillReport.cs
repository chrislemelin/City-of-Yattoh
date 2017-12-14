using Placeholdernamespace.Battle.Entities.AttributeStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillReport {

        private Dictionary<StatType, Stat> sourceBefore;
        public Dictionary<StatType, Stat> SourceBefore
        {
            get { return sourceBefore; }
        }

        private Dictionary<StatType, Stat> sourceAfter;
        public Dictionary<StatType, Stat> SourceAfter
        {
            get { return sourceAfter; }
        }

        private Dictionary<StatType, Stat> targetBefore;
        public Dictionary<StatType, Stat> TargetBefore
        {
            get { return targetBefore; }
        }

        private Dictionary<StatType, Stat> targetAfter;
        public Dictionary<StatType, Stat> TargetAfter
        {
            get { return targetAfter; }
        }

        public SkillReport(Dictionary<StatType, Stat> sourceBefore, Dictionary<StatType, Stat> sourceAfter,
            Dictionary<StatType, Stat> targetBefore, Dictionary<StatType, Stat> targetAfter)
        {
            this.sourceBefore = sourceBefore;
            this.sourceAfter = sourceAfter;
            this.targetBefore = targetBefore;
            this.targetAfter = targetAfter;
        }

    }
}