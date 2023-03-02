using UnityEngine;

namespace FluffyGameDev.Escapists.Quest
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Quest System/Quest")]
    public class QuestBlueprint : ScriptableObject
    {
        [SerializeField]
        private QuestObjectiveBlueprint m_RootObjective;
        public QuestObjectiveBlueprint RootObjective
        {
            get { return m_RootObjective; }
            set { m_RootObjective = value; }
        }

        public Quest InstantiateQuest()
        {
            QuestObjective questObjective = m_RootObjective?.InstantiateQuestObjective();
            return new(questObjective);
        }
    }
}
