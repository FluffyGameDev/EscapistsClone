using System;
using System.Collections.Generic;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class Inventory
    {
        private List<InventorySlot> m_Slot = new();

        public int slotCount => m_Slot.Count;

        public InventorySlot CreateSlot()
        {
            InventorySlot slot = new();
            m_Slot.Add(slot);
            return slot;
        }

        public void ForEachSlot(Action<InventorySlot> operation)
        {
            m_Slot.ForEach(operation);
        }

        public InventorySlot FindSlot(Predicate<InventorySlot> predicate)
        {
            return m_Slot.Find(predicate);
        }

        public void FilterSlots(Predicate<InventorySlot> predicate, List<InventorySlot> outputSlots)
        {
            foreach (InventorySlot slot in m_Slot)
            {
                if (predicate(slot))
                {
                    outputSlots.Add(slot);
                }
            }
        }
    }
}
