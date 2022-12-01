using FluffyGameDev.Escapists.InventorySystem;
using System;
using UnityEngine;

namespace FluffyGameDev.Escapists
{
    public class ToolItemBehaviourCreator : InventoryItemBehaviourCreator
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float m_ToolDamageRatio;

        public override InventoryItemBehaviour Create(InventoryItem owner)
        {
            return new ToolItemBehaviour(owner, m_ToolDamageRatio);
        }
    }

    public class ToolItemBehaviour : InventoryItemBehaviour
    {
        private float m_ToolDamageRatio;

        public ToolItemBehaviour(InventoryItem owner, float toolDamageRatio)
            : base(owner)
        {
            m_ToolDamageRatio = toolDamageRatio;
        }

        public void UseTool(WorldDataHolder worldDataHolder, Vector3Int interactionPosition)
        {
            /* TODO
            Types of tiles
            ...
            */

            worldDataHolder.DamageTile(interactionPosition, m_ToolDamageRatio);
        }
    }
}
