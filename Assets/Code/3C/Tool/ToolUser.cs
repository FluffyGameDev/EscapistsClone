using FluffyGameDev.Escapists.InventorySystem;
using FluffyGameDev.Escapists.Player;
using FluffyGameDev.Escapists.World;
using UnityEngine;

namespace FluffyGameDev.Escapists.Tool
{
    //TODO: rename
    public class ToolUser : MonoBehaviour
    {
        [SerializeField]
        private InventoryChannel m_InventoryChannel;
        [SerializeField]
        private PlayerChannel m_PlayerChannel;
        [SerializeField]
        private WorldChannel m_WorldChannel;

        private ToolItemBehaviour m_CurrentTool;
        public ToolItemBehaviour currentTool => m_CurrentTool;

        private void Awake()
        {
            m_PlayerChannel.OnToolEquip += OnToolEquip;
            m_WorldChannel.OnWorldInteraction += OnWorldInteraction;
        }

        private void OnDestroy()
        {
            m_WorldChannel.OnWorldInteraction -= OnWorldInteraction;
            m_PlayerChannel.OnToolEquip -= OnToolEquip;
        }

        private void OnToolEquip(ToolItemBehaviour tool)
        {
            m_CurrentTool = tool;
        }

        private void OnWorldInteraction(WorldDataHolder worldDataHolder, Vector3Int interactionPosition)
        {
            if (m_CurrentTool != null)
            {
                m_CurrentTool.UseTool(worldDataHolder, interactionPosition);

                DurabilityItemBehaviour durabilityBehaviour = m_CurrentTool.owner.FindBehaviour<DurabilityItemBehaviour>();
                if (durabilityBehaviour != null)
                {
                    durabilityBehaviour.ApplyUseDamage();

                    if (durabilityBehaviour.isBroken)
                    {
                        m_InventoryChannel.RaiseItemDestroy(m_CurrentTool.owner);
                        m_CurrentTool = null;
                    }
                }
            }
        }
    }
}
