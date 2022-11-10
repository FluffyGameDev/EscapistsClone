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

        public override InventoryItemBehaviour Create()
        {
            return this;
        }

        public void UseTool(WorldDataHolder worldDataHolder, Vector3Int interactionPosition)
        {
            /* TODO
            Amount of Damage
            Types of tiles
            ...
            */

            worldDataHolder.DamageTile(interactionPosition, 0.0f);
        }
    }
}
