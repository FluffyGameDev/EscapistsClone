using FluffyGameDev.Escapists.InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Crafting
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Crafting/Crafting Recipe Data")]
    public class CraftingRecipeData : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItemData> m_RequiredItems;
        [SerializeField]
        private int m_RequiredIntelligence;

        public List<InventoryItemData> requiredItems => m_RequiredItems;
        public int requiredIntelligence => m_RequiredIntelligence;
    }
}
