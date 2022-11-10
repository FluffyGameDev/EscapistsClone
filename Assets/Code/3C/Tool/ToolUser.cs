using FluffyGameDev.Escapists.Player;
using FluffyGameDev.Escapists.World;
using UnityEngine;

namespace FluffyGameDev.Escapists.Tool
{
    public class ToolUser : MonoBehaviour
    {
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
            m_CurrentTool?.UseTool(worldDataHolder, interactionPosition);
        }
    }
}
