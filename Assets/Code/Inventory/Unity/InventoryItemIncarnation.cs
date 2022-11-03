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
            inventoryItem ??= m_InventoryItemData.CreateItem();
            m_InventoryChannel.RaiseItemPickUp(inventoryItem);
        }
    }
}
