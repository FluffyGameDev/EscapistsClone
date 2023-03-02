using FluffyGameDev.Escapists.Core;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Quest
{
    public interface IQuestService : IService
    {
        List<Quest> ActiveQuests { get; }
        Quest BeginQuest(QuestBlueprint blueprint);
        void CancelQuest(Quest quest);
    }

    public class QuestService : MonoBehaviour, IQuestService
    {
        private List<Quest> m_ActiveQuests = new();
        public List<Quest> ActiveQuests => m_ActiveQuests;

        private void Awake()
        {
            ServiceLocator.RegisterService<IQuestService>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<IQuestService>();
        }

        public void Init()
        {
        }

        public void Shutdown()
        {
        }

        public Quest BeginQuest(QuestBlueprint blueprint)
        {
            Quest newQuest = blueprint.InstantiateQuest();

            StartQuestTracking(newQuest);
            newQuest.BeginQuest();

            return newQuest;
        }

        public void CancelQuest(Quest quest)
        {
            if (quest.Status != QuestStatus.Canceled)
            {
                quest.CancelQuest();
            }
        }

        private void OnQuestStatusChanged(Quest quest)
        {
            switch (quest.Status)
            {
                case QuestStatus.Canceled:
                case QuestStatus.Completed:
                case QuestStatus.Failed:
                {
                    StopQuestTracking(quest);
                    break;
                }
            }
        }

        private void StartQuestTracking(Quest quest)
        {
            quest.OnQuestStatusChanged += OnQuestStatusChanged;
            m_ActiveQuests.Add(quest);
        }

        private void StopQuestTracking(Quest quest)
        {
            quest.OnQuestStatusChanged -= OnQuestStatusChanged;
            m_ActiveQuests.Remove(quest);
        }
    }
}
