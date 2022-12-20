using UnityEngine;

namespace FluffyGameDev.Escapists.Stats
{
    [CreateAssetMenu(menuName = "FluffyGameDev/Escapists/Stats/Float Stat Descriptor")]
    public class FloatStatDescriptor : StatDescriptor
    {
        [SerializeField]
        private float m_DefaultValue;

        public override Stat CreateStat()
        {
            return new Stat(m_DefaultValue);
        }
    }
}
