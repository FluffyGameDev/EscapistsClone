using UnityEngine;

namespace FluffyGameDev.Escapists.World
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/World/Agent Job Identifier")]
    public class AgentJobIdentifier : ScriptableObject
    {
        public const int InvalidJobId = -1;

        [SerializeField]
        private int m_JobId;
        public int JobId => m_JobId;
    }
}