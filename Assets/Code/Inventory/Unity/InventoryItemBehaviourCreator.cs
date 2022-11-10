using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public abstract class InventoryItemBehaviourCreator : ScriptableObject
    {
        public abstract InventoryItemBehaviour Create();
    }
}
