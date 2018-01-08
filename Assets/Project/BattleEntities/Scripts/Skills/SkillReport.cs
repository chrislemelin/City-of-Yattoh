using Placeholdernamespace.Battle.Entities.AttributeStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillReport {

        private Stats sourceBefore;
        public Stats SourceBefore
        {
            get { return sourceBefore; }
        }

        private Stats sourceAfter;
        public Stats SourceAfter
        {
            get { return sourceAfter; }
        }

        private Stats targetBefore;
        public Stats TargetBefore
        {
            get { return targetBefore; }
        }

        private Stats targetAfter;
        public Stats TargetAfter
        {
            get { return targetAfter; }
        }

        public SkillReport(Stats sourceBefore, Stats sourceAfter, Stats targetBefore, Stats targetAfter)
        {
            this.sourceBefore = sourceBefore;
            this.sourceAfter = sourceAfter;
            this.targetBefore = targetBefore;
            this.targetAfter = targetAfter;
        }

    }
}