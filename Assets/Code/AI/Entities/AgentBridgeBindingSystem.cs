using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public partial struct AgentBridgeBindingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (agent, entity) in SystemAPI.Query<AgentComponent>()
                                          .WithNone<AgentBridgeComponent>()
                                          .WithEntityAccess())
            {
                int bridgeId = AgentBridgeBehaviourPool.Instance.AcquireInstance();
                ecb.AddComponent(entity, new AgentBridgeComponent { AgentBridgeBehaviourId = bridgeId });
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
