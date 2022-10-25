using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Inventory/Item Data")]
    public class InventoryItemData : ScriptableObject
    {
        [SerializeField]
        private string m_ItemName;
        public string itemName => m_ItemName;
    }
}
