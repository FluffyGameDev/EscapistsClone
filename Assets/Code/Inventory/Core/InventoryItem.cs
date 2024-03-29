using System;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.InventorySystem
{
    public class InventoryItem
    {
        public string itemName; // TODO: localization
        public Sprite itemIcon; // TODO: Use Addressable key
        public int itemID; // TODO: Use Addressable key

        public void AddBehaviour(InventoryItemBehaviour addedBehaviour)
        {
            if (!m_Behaviours.TryAdd(addedBehaviour.GetType(), addedBehaviour))
            {
                //TODO: error
            }
        }

        public T FindBehaviour<T>()
            where T : InventoryItemBehaviour
        {
            m_Behaviours.TryGetValue(typeof(T), out InventoryItemBehaviour foundBehaviour);
            return foundBehaviour as T;
        }

        private Dictionary<Type, InventoryItemBehaviour> m_Behaviours = new();
    }
}
