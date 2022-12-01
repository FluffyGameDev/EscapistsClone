using FluffyGameDev.Escapists.Core;
using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private InventoryItemData m_InventoryItemData;

        private void Start()
        {
            //TODO: cache created items
            InventoryItem item = m_InventoryItemData.CreateItem();
            ServiceLocator.LocateService<IInventoryItemIncarnationPool>()
                .AcquireIncarnation(transform.position, item);

            Destroy(gameObject);
        }
    }
}
