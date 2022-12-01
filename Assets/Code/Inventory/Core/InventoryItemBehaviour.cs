using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public abstract class InventoryItemBehaviour
    {
        private InventoryItem m_Owner;
        public InventoryItem owner => m_Owner;

        public InventoryItemBehaviour(InventoryItem owner)
        {
            m_Owner = owner;
        }
    }
}
