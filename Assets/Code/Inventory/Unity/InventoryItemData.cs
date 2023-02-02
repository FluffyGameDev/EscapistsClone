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
        private Sprite m_ItemIcon;
        public Sprite itemIcon => m_ItemIcon;

        [SerializeField]
        private int m_ItemID; //TODO: auto generate IDs
        public int itemID => m_ItemID;


        [SerializeField]
        private List<InventoryItemBehaviourCreator> m_behaviourCreators = new();
        public List<InventoryItemBehaviourCreator> behaviourCreators => m_behaviourCreators;

        public InventoryItem CreateItem()
        {
            InventoryItem item = new();
            item.itemID = m_ItemID;
            item.itemName = m_ItemName;
            item.itemIcon = m_ItemIcon;

            foreach (var behaviourCreator in m_behaviourCreators)
            {
                item.AddBehaviour(behaviourCreator.Create(item));
            }

            return item;
        }
    }
}
