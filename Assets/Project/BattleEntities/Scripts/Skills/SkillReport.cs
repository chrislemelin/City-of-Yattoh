using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.Entities.Passives;
using Placeholdernamespace.Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Placeholdernamespace.Battle.Entities.Skills
{
    public class SkillReport {

        public Stats sourceBefore;
        public Stats SourceBefore
        {
            get { return sourceBefore; }
        }

        public Stats sourceAfter;
        public Stats SourceAfter
        {
            get { return sourceAfter; }
        }

        public Stats targetBefore;
        public Stats TargetBefore
        {
            get { return targetBefore; }
        }

        public Stats targetAfter;
        public Stats TargetAfter
        {
            get { return targetAfter; }
        }

        public List<DamageDisplay> DamageDisplays = new List<DamageDisplay>();
        public List<TextDisplay> TextDisplays = new List<TextDisplay>();
        public List<Buff> Buffs = new List<Buff>();

        public SkillReport()
        {

        }

        public SkillReport(Stats sourceBefore, Stats sourceAfter, Stats targetBefore, Stats targetAfter)
        {
            this.sourceBefore = sourceBefore;
            this.sourceAfter = sourceAfter;
            this.targetBefore = targetBefore;
            this.targetAfter = targetAfter;
            targets.Add(new Tuple<Stats, Stats>(targetBefore, targetAfter));
        }

        public SkillReport(Stats sourceBefore, Stats sourceAfter, List<Tuple<Stats,Stats>> targets)
        {
            source.first = sourceBefore;
            source.second = sourceAfter;
            if(targets.Count > 0 )
            {
                targetBefore = targets[0].first;
                targetAfter = targets[1].second;
            }
            this.targets = targets;
        }

        public void AddTogether(SkillReport skillReport)
        {
            targets.AddRange(skillReport.targets);
        }

        public Tuple<Stats, Stats> source;

        public List<Tuple<Stats, Stats>> targets = new List<Tuple<Stats, Stats>>();

    }
}