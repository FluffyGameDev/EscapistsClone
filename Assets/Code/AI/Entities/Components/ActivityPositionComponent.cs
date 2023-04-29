using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public struct ActivityPositionComponent : IComponentData
    {
        public int ActivityId;
        public int AgentJobId;
        public int AgentRoleId;
        public int OwnerAgentId;
        public Entity ReservingEntity;
        public float IdleMinDuration;
        public float IdleMaxDuration;
        public bool CanBeReserved;
    }
}
