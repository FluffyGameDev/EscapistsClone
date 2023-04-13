using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public struct ActivityPositionComponent : IComponentData
    {
        public int ActivityId;
        public Entity ReservingEntity;
        public float IdleMinDuration;
        public float IdleMaxDuration;
    }
}
