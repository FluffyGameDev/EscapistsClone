using System;
using System.Collections.Generic;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class Inventory
    {
        private List<InventorySlot> m_Slots = new();

        public List<InventorySlot> slots => m_Slots;
        public int slotCount => m_Slots.Count;

        public InventorySlot CreateSlot()
        {
            InventorySlot slot = new();
            m_Slots.Add(slot);
            return slot;
        }

        public void ForEachSlot(Action<InventorySlot> operation)
        {
            m_Slots.ForEach(operation);
        }

        public InventorySlot FindSlot(Predicate<InventorySlot> predicate)
        {
            return m_Slots.Find(predicate);
        }

        public void FilterSlots(Predicate<InventorySlot> predicate, List<InventorySlot> outputSlots)
        {
            foreach (InventorySlot slot in m_Slots)
            {
                if (predicate(slot))
                {
                    outputSlots.Add(slot);
                }
            }
        }
    }
}
