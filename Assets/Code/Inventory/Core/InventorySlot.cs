using System;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class InventorySlot
    {
        public event Action<InventorySlot> OnSlotModified;

        public InventoryItem Item => m_Item;
        public uint Quantity => m_Quantity;

        public bool StoreItem(InventoryItem item, uint quantity)
        {
            if (m_Item == item || m_Item == null)
            {
                m_Item = item;
                m_Quantity += quantity;
                OnSlotModified.Invoke(this);
                return true;
            }

            return false;
        }

        public void ClearSlot()
        {
            if (m_Item != null || m_Quantity > 0)
            {
                m_Item = null;
                m_Quantity = 0;
                OnSlotModified.Invoke(this);
            }
        }

        public bool MoveItemTo(InventorySlot targetSlot, uint quantity)
        {
            if (targetSlot == null || quantity > m_Quantity || m_Quantity == 0 && targetSlot.m_Quantity == 0)
            {
                return false;
            }
            else
            {
                if (targetSlot.m_Item == m_Item || targetSlot.m_Item == null)
                {
                    targetSlot.m_Item = m_Item;
                    targetSlot.m_Quantity += quantity;
                    m_Quantity -= quantity;

                    if (m_Quantity == 0)
                    {
                        m_Item = null;
                    }
                }
                else if (m_Quantity == quantity)
                {
                    InventoryItem tmpItem = m_Item;
                    m_Item = targetSlot.m_Item;
                    targetSlot.m_Item = tmpItem;

                    uint tmpQuantity = m_Quantity;
                    m_Quantity = targetSlot.m_Quantity;
                    targetSlot.m_Quantity = tmpQuantity;
                }
                else
                {
                    return false;
                }
            }
            OnSlotModified?.Invoke(this);
            targetSlot.OnSlotModified?.Invoke(targetSlot);
            return true;
        }

        private InventoryItem m_Item;
        private uint m_Quantity;
    }
}
