using FluffyGameDev.Escapists.InventorySystem;

namespace FluffyGameDev.Escapists.UI
{
    public class CraftingInventoryActionStrategy : InventoryActionStrategy
    {
        UICraftingChannel m_UICraftingChannel;

        public CraftingInventoryActionStrategy(UICraftingChannel uiCraftingChannel)
        {
            m_UICraftingChannel = uiCraftingChannel;
        }

        public void RaiseMainAction(InventorySlot slot, ToolItemBehaviour currentToolBehaviour)
        {
            m_UICraftingChannel?.RaiseRequestAddItemToCraft(slot.Item);
            slot.ClearSlot();
        }

        public void RaiseSecondaryAction(InventorySlot slot, ToolItemBehaviour currentToolBehaviour)
        {
        }
    }
}
