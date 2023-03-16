using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Quest
{
    public class GroupQuestObjectiveBlueprint : QuestObjectiveBlueprint
    {
        [SerializeField]
        private List<QuestObjectiveBlueprint> m_ObjectiveGroup;
        [SerializeField]
        private int m_TargetObjectiveCount;

        public override List<QuestObjectiveBlueprint> SubObjectives => m_ObjectiveGroup;
        public override bool HasSubObjectives => true;

        public List<QuestObjectiveBlueprint> ObjectiveQroup => m_ObjectiveGroup;

        public override QuestObjective InstantiateQuestObjective()
        {
            List<QuestObjective> objectives = new(m_ObjectiveGroup.Count);
            foreach (var objectiveBlueprint in m_ObjectiveGroup)
            {
                objectives.Add(objectiveBlueprint.InstantiateQuestObjective());
            }
            return new GroupQuestObjective(objectives, m_TargetObjectiveCount);
        }
    }
}
