using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Quest
{
    public class GroupQuestObjectiveBlueprint : QuestObjectiveBlueprint
    {
        [SerializeField]
        private List<QuestObjectiveBlueprint> m_ObjectiveQroup;
        [SerializeField]
        private int m_TargetObjectiveCount;

        public List<QuestObjectiveBlueprint> ObjectiveQroup => m_ObjectiveQroup;

        public override QuestObjective InstantiateQuestObjective()
        {
            List<QuestObjective> objectives = new(m_ObjectiveQroup.Count);
            foreach (var objectiveBlueprint in m_ObjectiveQroup)
            {
                objectives.Add(objectiveBlueprint.InstantiateQuestObjective());
            }
            return new GroupQuestObjective(objectives, m_TargetObjectiveCount);
        }
    }
}
