using FluffyGameDev.Escapists.Core;
using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private InventoryItemData m_InventoryItemData;

        private void Awake()
        {
            //TODO: cache created items

            ServiceLocator.WaitUntilReady<IInventoryItemIncarnationPool>(OnServiceReady);

        }

        private void OnServiceReady()
        {
            InventoryItem item = m_InventoryItemData.CreateItem();
            ServiceLocator.LocateService<IInventoryItemIncarnationPool>()
                .AcquireIncarnation(WorldUtils.SnapToGrid(transform.position), item);

            Destroy(gameObject);
        }
    }
}
