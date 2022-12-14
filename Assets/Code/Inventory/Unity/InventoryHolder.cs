using FluffyGameDev.Escapists.Core;
using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class InventoryHolder : MonoBehaviour
    {
        [SerializeField]
        private int m_SlotCount;
        [SerializeField]
        private InventoryChannel m_InventoryChannel;

        public Inventory inventory { get; private set; }

        private void Awake()
        {
            m_InventoryChannel.OnItemPickUp += OnItemPickUp;
            m_InventoryChannel.OnItemDestroy += OnItemDestroy;
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
            m_InventoryChannel.OnItemDestroy -= OnItemDestroy;
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

        private void OnItemDestroy(InventoryItem item)
        {
            InventorySlot bestSlot = inventory.FindSlot(slot => slot.Item == item);
            if (bestSlot != null)
            {
                bestSlot.ClearSlot();
            }
        }

        private void OnItemDrop(InventorySlot slot)
        {
            if (slot.Item != null)
            {
                ServiceLocator.LocateService<IInventoryItemIncarnationPool>()
                    .AcquireIncarnation(WorldUtils.SnapToGrid(transform.position), slot.Item);
                slot.ClearSlot();
            }
        }
    }
}
