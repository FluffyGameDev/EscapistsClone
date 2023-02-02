using FluffyGameDev.Escapists.InventorySystem;
using FluffyGameDev.Escapists.Player;

namespace FluffyGameDev.Escapists.UI
{
    public class DefaultInventoryActionStrategy : InventoryActionStrategy
    {
        private PlayerChannel m_PlayerChannel;
        private InventoryChannel m_InventoryChannel;

        public DefaultInventoryActionStrategy(PlayerChannel playerChannel, InventoryChannel inventoryChannel)
        {
            m_PlayerChannel = playerChannel;
            m_InventoryChannel = inventoryChannel;
        }

        public void RaiseMainAction(InventorySlot slot, ToolItemBehaviour currentToolBehaviour)
        {
            m_PlayerChannel.RaiseToolEquip(currentToolBehaviour);
        }

        public void RaiseSecondaryAction(InventorySlot slot, ToolItemBehaviour currentToolBehaviour)
        {
            if (currentToolBehaviour != null)
            {
                m_PlayerChannel.RaiseToolEquip(null);
            }
            m_InventoryChannel.RaiseItemDrop(slot);
        }
    }
}
