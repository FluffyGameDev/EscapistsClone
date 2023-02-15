using System;
using UnityEngine;
using FluffyGameDev.Escapists.InventorySystem;

namespace FluffyGameDev.Escapists.Crafting
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Crafting/Crafting Channel")]
    public class CraftingChannel : ScriptableObject
    {
        public event Action<InventoryItem> onCraftSucceeded;
        public event Action<int> onCraftFailed;

        public void RaiseCraftSucceeded(InventoryItem item)
        {
            onCraftSucceeded?.Invoke(item);
        }

        public void RaiseCraftFailed(int missingIntellect)
        {
            onCraftFailed?.Invoke(missingIntellect);
        }
    }
}
