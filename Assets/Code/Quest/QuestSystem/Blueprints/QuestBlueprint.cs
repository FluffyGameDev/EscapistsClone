using FluffyGameDev.Escapists.Core;
using UnityEngine;

namespace FluffyGameDev.Escapists.Quest
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Quest System/Quest")]
    public class QuestBlueprint : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        private QuestObjectiveBlueprint m_RootObjective;
        public QuestObjectiveBlueprint RootObjective
        {
            get { return m_RootObjective; }
            set { m_RootObjective = value; }
        }

        [SerializeField]
        private string m_QuestName; //TODO: localize
        public string QuestName
        {
            get { return m_QuestName; }
            set { m_QuestName = value; }
        }

        public Quest InstantiateQuest()
        {
            QuestObjective questObjective = m_RootObjective?.InstantiateQuestObjective();
            return new(questObjective, m_QuestName);
        }

        [ContextMenu("Add Quest to Quest Log")]
        private void AddQuestToQuestlog()
        {
            IQuestService questService = ServiceLocator.LocateService<IQuestService>();
            if (questService != null)
            {
                questService.BeginQuest(this);
            }
        }
    }
}
