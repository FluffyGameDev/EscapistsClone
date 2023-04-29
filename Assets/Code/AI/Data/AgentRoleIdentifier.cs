using UnityEngine;

namespace FluffyGameDev.Escapists.World
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/World/Agent Role Identifier")]
    public class AgentRoleIdentifier : ScriptableObject
    {
        public const int InvalidRoleId = -1;

        [SerializeField]
        private int m_RoleId;
        public int RoleId => m_RoleId;
    }
}