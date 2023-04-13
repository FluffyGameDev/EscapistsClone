using Unity.Entities;
using Unity.Transforms;

namespace FluffyGameDev.Escapists.AI
{
    public partial struct AgentBridgeUpdaterSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (agent, agentBridge, worldTransform) in SystemAPI.Query<AgentComponent, AgentBridgeComponent, WorldTransform>())
            {
                AgentBridgeBehaviour bridgeBehaviour = AgentBridgeBehaviourPool.Instance.GetInstance(agentBridge.AgentBridgeBehaviourId);
                bridgeBehaviour.UpdateBridgeData(agent.State, worldTransform.Position);
            }
        }
    }
}
