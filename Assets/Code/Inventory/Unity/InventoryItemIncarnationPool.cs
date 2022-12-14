using FluffyGameDev.Escapists.Core;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public interface IInventoryItemIncarnationPool : IService
    {
        InventoryItemIncarnation AcquireIncarnation(Vector3 position, InventoryItem item);
        void ReleaseIncarnation(InventoryItemIncarnation incarnation);
    }

    public class InventoryItemIncarnationPool : MonoBehaviour, IInventoryItemIncarnationPool
    {
        [SerializeField]
        private InventoryItemIncarnation m_InventoryItemIncarnationPrefab;
        [SerializeField]
        private Sprite m_PlaceholderItemSprite;

        private Stack<InventoryItemIncarnation> m_AvailableIncarnations = new();

        private void Awake()
        {
            ServiceLocator.RegisterService<IInventoryItemIncarnationPool>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<IInventoryItemIncarnationPool>();
        }

        public void Init()
        {
        }

        public void Shutdown()
        {
        }

        public InventoryItemIncarnation AcquireIncarnation(Vector3 position, InventoryItem item)
        {
            InventoryItemIncarnation foundIncarnation;
            if (m_AvailableIncarnations.Count == 0)
            {
                foundIncarnation = Instantiate(m_InventoryItemIncarnationPrefab, transform);
            }
            else
            {
                foundIncarnation = m_AvailableIncarnations.Pop();
                foundIncarnation.gameObject.SetActive(true);
            }

            foundIncarnation.transform.position = position;
            foundIncarnation.inventoryItem = item;

            return null;
        }

        public void ReleaseIncarnation(InventoryItemIncarnation incarnation)
        {
            //TODO: Check parent ?
            incarnation.gameObject.SetActive(false);
            m_AvailableIncarnations.Push(incarnation);
        }
    }
}
