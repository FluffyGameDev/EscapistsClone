using FluffyGameDev.Escapists.InventorySystem;
using System;
using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/UI/UI Crafting Channel")]
    public class UICraftingChannel : ScriptableObject
    {
        public event Action<InventoryItem> onRequestAddItemToCraft;

        public void RaiseRequestAddItemToCraft(InventoryItem item)
        {
            onRequestAddItemToCraft?.Invoke(item);
        }
    }
}
