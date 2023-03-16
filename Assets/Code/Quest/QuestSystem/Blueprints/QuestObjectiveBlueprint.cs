using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Quest
{
    public abstract class QuestObjectiveBlueprint : ScriptableObject
    {
        [SerializeField]
        private string m_ObjectiveDescription;
        public string ObjectiveDescription => m_ObjectiveDescription;

        public abstract QuestObjective InstantiateQuestObjective();

        public virtual List<QuestObjectiveBlueprint> SubObjectives => null;
        public virtual bool HasSubObjectives => false;
    }
}
