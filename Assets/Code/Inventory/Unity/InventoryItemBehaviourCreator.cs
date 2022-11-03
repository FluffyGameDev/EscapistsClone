using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public interface InventoryItemBehaviourCreator
    {
        InventoryItemBehaviour Create();
    }
}
