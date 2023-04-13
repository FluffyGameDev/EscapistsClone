using Unity.Entities;
using Unity.Mathematics;

namespace FluffyGameDev.Escapists.AI
{
    public struct MovementTargetComponent : IComponentData, IEnableableComponent
    {
        public float3 TargetPosition;
        public float Speed;
    }
}
