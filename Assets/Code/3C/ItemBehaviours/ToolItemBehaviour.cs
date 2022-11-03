using FluffyGameDev.Escapists.InventorySystem;
using System;
using UnityEngine;

namespace FluffyGameDev.Escapists
{
    [Serializable]
    public class ToolItemBehaviour : InventoryItemBehaviourCreator, InventoryItemBehaviour
    {
        //TODO: Can we get away with 1 class ?

        [SerializeField]
        private string m_ToolName;

        public InventoryItemBehaviour Create()
        {
            return this;
        }

        public void UseTool()
        {
            Debug.Log($"Used Tool ({m_ToolName}).");
        }
    }
}
