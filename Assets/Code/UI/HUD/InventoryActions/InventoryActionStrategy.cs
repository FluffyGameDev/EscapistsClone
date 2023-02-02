using FluffyGameDev.Escapists.InventorySystem;

namespace FluffyGameDev.Escapists.UI
{
    public interface InventoryActionStrategy
    {
        void RaiseMainAction(InventorySlot slot, ToolItemBehaviour currentToolBehaviour);
        void RaiseSecondaryAction(InventorySlot slot, ToolItemBehaviour currentToolBehaviour);
    }
}
