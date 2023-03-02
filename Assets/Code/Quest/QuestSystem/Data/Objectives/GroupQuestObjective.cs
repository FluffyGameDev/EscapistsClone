using System;
using System.Collections.Generic;

namespace FluffyGameDev.Escapists.Quest
{
    public class GroupQuestObjective : QuestObjective
    {
        private List<QuestObjective> m_ObjectiveGroup;
        private int m_TargetObjectiveCount = 0;
        private int m_CompletedObjectiveCount = 0;

        public GroupQuestObjective(List<QuestObjective> objectiveGroup)
        {
            m_ObjectiveGroup = objectiveGroup;
            m_TargetObjectiveCount = objectiveGroup.Count;
        }

        public GroupQuestObjective(List<QuestObjective> objectiveGroup, int targetObjectiveCount)
        {
            m_ObjectiveGroup = objectiveGroup;
            m_TargetObjectiveCount = Math.Clamp(targetObjectiveCount, 1, objectiveGroup.Count);
        }

        protected override void OnBeginQuestObjective()
        {
            m_CompletedObjectiveCount = 0;
            foreach (var subObjective in m_ObjectiveGroup)
            {
                subObjective.OnQuestObjectiveStatusChanged += OnQuestObjectiveStatusChangedCallback;
                subObjective.BeginQuestObjective();
            }
        }

        protected override void OnEndQuestObjective()
        {
            foreach (var subObjective  in m_ObjectiveGroup)
            {
                if (subObjective.Status != QuestObjectiveStatus.InProgress)
                {
                    subObjective.OnQuestObjectiveStatusChanged -= OnQuestObjectiveStatusChangedCallback;
                    subObjective.CancelQuestObjective();
                }
            }
        }

        private void OnQuestObjectiveStatusChangedCallback(QuestObjective objective)
        {
            objective.OnQuestObjectiveStatusChanged -= OnQuestObjectiveStatusChangedCallback;
            switch (objective.Status)
            {
                case QuestObjectiveStatus.Completed:
                {
                    ++m_CompletedObjectiveCount;
                    if (m_CompletedObjectiveCount >= m_TargetObjectiveCount)
                    {
                        NotifyQuestObjectiveCompleted();
                    }
                    break;
                }
                case QuestObjectiveStatus.Failed:
                {
                    NotifyQuestObjectiveFailed();
                    break;
                }
            }
        }
    }
}
