using FluffyGameDev.Escapists.InventorySystem;
using FluffyGameDev.Escapists.Quest;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FluffyGameDev.Escapists
{
    public class ToolUsedQuestObjectiveBlueprint : QuestObjectiveBlueprint
    {
        [SerializeField]
        private ToolUsedGameplayEvent m_ToolUsedEvent;
        [SerializeField]
        InventoryItemData m_WatchedItem;
        [SerializeField]
        TileBase m_WatchedTileType;

        public override QuestObjective InstantiateQuestObjective()
        {
            return new ToolUsedQuestObjective(m_ToolUsedEvent, m_WatchedItem.itemID, m_WatchedTileType);
        }
    }
}
