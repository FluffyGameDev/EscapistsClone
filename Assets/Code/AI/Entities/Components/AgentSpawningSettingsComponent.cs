using Unity.Collections;
using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public struct AgentSpawningSettingsComponent : IComponentData
    {
        public int CellActivityId;
        public int InmateRoleId;
        public int GuardRoleId;
        public float AgentSpeed;
        public NativeArray<int> AvailableJobIds;
    }
}
