using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Quest
{
    public class SequenceQuestObjectiveBlueprint : QuestObjectiveBlueprint
    {
        [SerializeField]
        [HideInInspector]
        private List<QuestObjectiveBlueprint> m_ObjectiveSequence;
        public override string SubObjectivesPropertyName => "m_ObjectiveSequence";
        public override List<QuestObjectiveBlueprint> SubObjectives => m_ObjectiveSequence;
        public override bool HasSubObjectives => true;

        public override QuestObjective InstantiateQuestObjective()
        {
            List<QuestObjective> objectives = new(m_ObjectiveSequence.Count);
            foreach (var objectiveBlueprint in m_ObjectiveSequence)
            {
                objectives.Add(objectiveBlueprint.InstantiateQuestObjective());
            }
            return new SequenceQuestObjective(objectives);
        }
    }
}
