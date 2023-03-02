using System.Collections.Generic;

namespace FluffyGameDev.Escapists.Quest
{
    public class SequenceQuestObjective : QuestObjective
    {
        private List<QuestObjective> m_ObjectiveSequence;
        private int m_CurrentObjectiveIndex = 0;

        public SequenceQuestObjective(List<QuestObjective> objectiveSequence)
        {
            m_ObjectiveSequence = objectiveSequence;
        }

        protected override void OnBeginQuestObjective()
        {
            BeginSubStep(0);
        }

        protected override void OnEndQuestObjective()
        {
            if (m_CurrentObjectiveIndex < m_ObjectiveSequence.Count)
            {
                var subObjective = m_ObjectiveSequence[m_CurrentObjectiveIndex];
                if (subObjective.Status != QuestObjectiveStatus.InProgress)
                {
                    subObjective.OnQuestObjectiveStatusChanged -= OnQuestObjectiveStatusChangedCallback;
                    subObjective.CancelQuestObjective();
                }
            }
        }

        private void BeginSubStep(int stepIndex)
        {
            m_CurrentObjectiveIndex = stepIndex;
            if (m_CurrentObjectiveIndex < m_ObjectiveSequence.Count)
            {
                var subObjective = m_ObjectiveSequence[m_CurrentObjectiveIndex];

                subObjective.OnQuestObjectiveStatusChanged += OnQuestObjectiveStatusChangedCallback;
                subObjective.BeginQuestObjective();
            }
            else
            {
                NotifyQuestObjectiveCompleted();
            }
        }

        private void OnQuestObjectiveStatusChangedCallback(QuestObjective objective)
        {
            objective.OnQuestObjectiveStatusChanged -= OnQuestObjectiveStatusChangedCallback;
            switch (objective.Status)
            {
                case QuestObjectiveStatus.Completed:
                {
                    BeginSubStep(m_CurrentObjectiveIndex + 1);
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
