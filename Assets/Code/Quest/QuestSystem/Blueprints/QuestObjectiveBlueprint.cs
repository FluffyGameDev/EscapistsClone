using UnityEngine;

namespace FluffyGameDev.Escapists.Quest
{
    public abstract class QuestObjectiveBlueprint : ScriptableObject
    {
        public abstract QuestObjective InstantiateQuestObjective();
    }
}
