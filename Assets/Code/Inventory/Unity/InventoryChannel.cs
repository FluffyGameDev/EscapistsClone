using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Inventory/Inventory Channel")]
    public class InventoryChannel : ScriptableObject
    {
        public delegate void ItemPickUpCallback(InventoryItem item);
        public delegate void ItemDestroyCallback(InventoryItem item);
        public delegate void ItemDropCallback(InventorySlot slot);

        public event ItemPickUpCallback OnItemPickUp;
        public event ItemDestroyCallback OnItemDestroy;
        public event ItemDropCallback OnItemDrop;

        public void RaiseItemPickUp(InventoryItem item)
        {
            OnItemPickUp?.Invoke(item);
        }

        public void RaiseItemDestroy(InventoryItem item)
        {
            OnItemDestroy?.Invoke(item);
        }

        public void RaiseItemDrop(InventorySlot slot)
        {
            OnItemDrop?.Invoke(slot);
        }
    }
}
