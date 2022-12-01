using FluffyGameDev.Escapists.Core;
using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class InventoryItemIncarnation : MonoBehaviour
    {
        [SerializeField]
        private InventoryChannel m_InventoryChannel;

        private SpriteRenderer m_SpriteRenderer;

        private InventoryItem m_InventoryItem;
        public InventoryItem inventoryItem
        {
            get => m_InventoryItem;
            set
            {
                if (m_InventoryItem != value)
                {
                    m_InventoryItem = value;

                    m_SpriteRenderer.sprite = m_InventoryItem.itemIcon;
                }
            }
        }

        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PickUpItem()
        {
            m_InventoryChannel.RaiseItemPickUp(inventoryItem);

            ServiceLocator.LocateService<IInventoryItemIncarnationPool>()
                .ReleaseIncarnation(this);
        }
    }
}
