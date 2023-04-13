using UnityEngine;

namespace FluffyGameDev.Escapists.World
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/World/Activity Identifier")]
    public class ActivityIdentifier : ScriptableObject
    {
        [SerializeField]
        private int m_ActivityId;
        public int ActivityId => m_ActivityId;
    }
}
