using UnityEngine.Tilemaps;

namespace FluffyGameDev.Escapists
{
    public class ToolUsedQuestObjective : GameplayEventQuestObjective<ToolItemBehaviour, TileBase>
    {
        int m_WatchedItemId;
        TileBase m_WatchedTileType;

        public ToolUsedQuestObjective(ToolUsedGameplayEvent toolUsedEvent, int watchedItemId, TileBase watchedTileType)
            : base(toolUsedEvent)
        {
            m_WatchedItemId = watchedItemId;
            m_WatchedTileType = watchedTileType;
        }

        protected override bool IsGameplayEventValid(ToolItemBehaviour argument1, TileBase argument2)
        {
            return argument1.owner.itemID == m_WatchedItemId && argument2 == m_WatchedTileType;
        }
    }
}
