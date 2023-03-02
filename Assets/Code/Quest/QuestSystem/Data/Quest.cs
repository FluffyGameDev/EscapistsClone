using System;

namespace FluffyGameDev.Escapists.Quest
{
    public enum QuestStatus
    {
        Pending,
        InProgress,
        Failed,
        Completed,
        Canceled
    }

    public class Quest
    {
        public event Action<Quest> OnQuestStatusChanged;

        private QuestStatus m_Status;
        public QuestStatus Status => m_Status;

        private QuestObjective m_RootObjective;

        // TODO: rewards

        public Quest(QuestObjective root)
        {
            m_Status = QuestStatus.Pending;
            m_RootObjective = root;
        }

        public void BeginQuest()
        {
            if (m_Status == QuestStatus.Pending)
            {
                m_Status = QuestStatus.InProgress;

                m_RootObjective.OnQuestObjectiveStatusChanged += OnQuestObjectiveStatusChanged;
                m_RootObjective.BeginQuestObjective();

                OnQuestStatusChanged?.Invoke(this);
            }
        }

        public void CancelQuest()
        {
            if (m_Status == QuestStatus.InProgress)
            {
                m_Status = QuestStatus.Canceled;
                m_RootObjective.CancelQuestObjective();
                OnQuestStatusChanged?.Invoke(this);
            }
        }

        public void OnEndQuest()
        {
            m_RootObjective.OnQuestObjectiveStatusChanged -= OnQuestObjectiveStatusChanged;
        }

        private void OnQuestObjectiveStatusChanged(QuestObjective objective)
        {
            switch (objective.Status)
            {
                case QuestObjectiveStatus.Completed:
                {
                    m_Status = QuestStatus.Completed;
                    OnEndQuest();
                    OnQuestStatusChanged?.Invoke(this);
                    break;
                }

                case QuestObjectiveStatus.Failed:
                {
                    m_Status = QuestStatus.Failed;
                    OnEndQuest();
                    OnQuestStatusChanged?.Invoke(this);
                    break;
                }
            }
        }
    }
}
