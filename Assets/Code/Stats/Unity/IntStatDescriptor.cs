using UnityEngine;

namespace FluffyGameDev.Escapists.Stats
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Stats/Int Stat Descriptor")]
    public class IntStatDescriptor : StatDescriptor
    {
        [SerializeField]
        private int m_DefaultValue;

        public override Stat CreateStat()
        {
            return new Stat(m_DefaultValue);
        }
    }
}
