using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Inventory/Item Data")]
    public class InventoryItemData : ScriptableObject
    {
        [SerializeField]
        private string m_ItemName;
        public string itemName => m_ItemName;

        [SerializeField]
        private List<InventoryItemBehaviourCreator> m_behaviourCreators = new();
        public List<InventoryItemBehaviourCreator> behaviourCreators => m_behaviourCreators;

        public InventoryItem CreateItem()
        {
            InventoryItem item = new();
            item.itemName = m_ItemName;

            foreach (var behaviourCreator in m_behaviourCreators)
            {
                item.AddBehaviour(behaviourCreator.Create());
            }

            return item;
        }
    }
}
