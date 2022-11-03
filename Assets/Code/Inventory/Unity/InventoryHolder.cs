using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class InventoryHolder : MonoBehaviour
    {
        [SerializeField]
        private int m_SlotCount;
        [SerializeField]
        private InventoryChannel m_InventoryChannel;
        [SerializeField]
        private InventoryItemIncarnation m_InventoryItemIncarnationPrefab;

        public Inventory inventory { get; private set; }

        private void Awake()
        {
            m_InventoryChannel.OnItemPickUp += OnItemPickUp;
            m_InventoryChannel.OnItemDrop += OnItemDrop;

            inventory = new();
            for (int i = 0; i < m_SlotCount; ++i)
            {
                inventory.CreateSlot();
            }
        }

        private void OnDestroy()
        {
            m_InventoryChannel.OnItemDrop -= OnItemDrop;
            m_InventoryChannel.OnItemPickUp -= OnItemPickUp;
        }

        private void OnItemPickUp(InventoryItem item)
        {
            InventorySlot bestSlot = inventory.FindSlot(slot => slot.Item == null);
            if (bestSlot != null)
            {
                bestSlot.StoreItem(item, 1);
            }
        }

        private void OnItemDrop(InventorySlot slot)
        {
            if (slot.Item != null)
            {
                InventoryItemIncarnation itemIncarnation = Instantiate(m_InventoryItemIncarnationPrefab);
                itemIncarnation.transform.position = transform.position;
                itemIncarnation.inventoryItem = slot.Item;
                slot.ClearSlot();
            }
        }
    }
}
