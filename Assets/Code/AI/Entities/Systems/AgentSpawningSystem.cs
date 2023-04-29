using FluffyGameDev.Escapists.World;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace FluffyGameDev.Escapists.AI
{
    [BurstCompile]
    public partial struct AgentSpawningSystem : ISystem
    {
        //TODO: Not great solution. Find a better way to reserve bed for player.
        private const int k_ReservedActivityId = 66;


        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            AgentSpawningSettingsComponent spawningSettings = SystemAPI.GetSingleton<AgentSpawningSettingsComponent>();

            int agentId = 0;
            int jobIndex = 0;
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (activity, worldTransform) in SystemAPI.Query<ActivityPositionComponent, WorldTransform>())
            {
                if (activity.ActivityId == spawningSettings.CellActivityId && activity.OwnerAgentId != k_ReservedActivityId)
                {
                    int jobId = jobIndex < spawningSettings.AvailableJobIds.Length ? spawningSettings.AvailableJobIds[jobIndex] : AgentJobIdentifier.InvalidJobId;
                    ++jobIndex;

                    Entity newCharacter = ecb.CreateEntity();
                    ecb.AddComponent(newCharacter, new AgentComponent() { AgentId = agentId, AgentRoleId = spawningSettings.InmateRoleId, AgentJobId = jobId });
                    ecb.AddComponent(newCharacter, new MovementTargetComponent() { Speed = spawningSettings.AgentSpeed, IsActive = false });
                    ecb.AddComponent(newCharacter, new WorldTransform() { Position = worldTransform.Position });
                    ++agentId;
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            state.Enabled = false;
        }
    }
}
