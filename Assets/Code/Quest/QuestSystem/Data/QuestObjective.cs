using System;

namespace FluffyGameDev.Escapists.Quest
{
    public enum QuestObjectiveStatus
    {
        Pending,
        InProgress,
        Failed,
        Completed,
        Canceled
    }

    public abstract class QuestObjective
    {
        public event Action<QuestObjective> OnQuestObjectiveStatusChanged;

        private QuestObjectiveStatus m_Status;
        public QuestObjectiveStatus Status => m_Status;

        public void BeginQuestObjective()
        {
            if (m_Status == QuestObjectiveStatus.Pending)
            {
                m_Status = QuestObjectiveStatus.InProgress;
                OnQuestObjectiveStatusChanged?.Invoke(this);
                OnBeginQuestObjective();
            }
        }

        public void CancelQuestObjective()
        {
            if (m_Status == QuestObjectiveStatus.InProgress)
            {
                m_Status = QuestObjectiveStatus.Canceled;
                OnQuestObjectiveStatusChanged?.Invoke(this);
                OnEndQuestObjective();
            }
        }

        protected void NotifyQuestObjectiveCompleted()
        {
            if (m_Status == QuestObjectiveStatus.InProgress)
            {
                m_Status = QuestObjectiveStatus.Completed;
                OnQuestObjectiveStatusChanged?.Invoke(this);
                OnEndQuestObjective();
            }
        }

        protected void NotifyQuestObjectiveFailed()
        {
            if (m_Status == QuestObjectiveStatus.InProgress)
            {
                m_Status = QuestObjectiveStatus.Failed;
                OnQuestObjectiveStatusChanged?.Invoke(this);
                OnEndQuestObjective();
            }
        }

        protected virtual void OnBeginQuestObjective()
        {
        }

        protected virtual void OnEndQuestObjective()
        {
        }
    }
}
