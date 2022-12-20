using UnityEngine;

namespace FluffyGameDev.Escapists.Stats
{
    public abstract class StatDescriptor : ScriptableObject, StatData
    {
        public abstract Stat CreateStat();
    }
}
