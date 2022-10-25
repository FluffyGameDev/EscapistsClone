using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class InventoryItemIncarnation : MonoBehaviour
    {
        [SerializeField]
        private InventoryChannel m_InventoryChannel;
        [SerializeField]
        private InventoryItemData m_InventoryItemData;

        public InventoryItem inventoryItem;

        public void PickUpItem()
        {
            if (inventoryItem == null)
            {
                inventoryItem = new();
                inventoryItem.itemName = m_InventoryItemData.itemName;
                //TODO: cache items
            }
            m_InventoryChannel.RaiseItemPickUp(inventoryItem);
        }
    }
}
