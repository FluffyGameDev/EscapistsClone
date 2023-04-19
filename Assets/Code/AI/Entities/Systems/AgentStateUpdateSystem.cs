using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace FluffyGameDev.Escapists.AI
{
    //[BurstCompile]
    public partial struct AgentStateUpdateSystem : ISystem
    {
        private const int k_RandomSeed = 1234; //TODO: find a better seed
        private Random m_Random;

        //[BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            m_Random = new Random(k_RandomSeed);
        }

        //[BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //TODO: move to a job
            EntityManager em = state.EntityManager;
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            DynamicBuffer<WorldActivityStepBufferElement> activityStepBuffer = SystemAPI.GetSingletonBuffer<WorldActivityStepBufferElement>();

            foreach (var (agent, movementTarget, worldTransform, agentEntity) in
                SystemAPI.Query<RefRW<AgentComponent>, RefRW<MovementTargetComponent>, WorldTransform>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                .WithEntityAccess())
            {
                switch (agent.ValueRO.State)
                {
                    case AgentState.Idle:
                    {
                        if (Time.time > agent.ValueRO.IdleEndTime)
                        {
                            // TODO: switch to path finding component when implemented
                            agent.ValueRW.State = AgentState.Walk;
                            movementTarget.ValueRW.TargetPosition = ComputeNextTarget(ref state, in worldTransform, ref agent.ValueRW, ref activityStepBuffer);
                            ecb.SetComponentEnabled<MovementTargetComponent>(agentEntity, true);
                        }
                        break;
                    }
                    case AgentState.Walk:
                    {
                        // TODO: switch to path finding component when implemented
                        if (!em.IsComponentEnabled<MovementTargetComponent>(agentEntity))
                        {
                            agent.ValueRW.IdleEndTime = Time.time + agent.ValueRO.NextIdleDuration;
                            agent.ValueRW.State = AgentState.Idle;
                        }
                        break;
                    }
                    case AgentState.Combat:
                    {
                        //TODO: implement when combat done
                        break;
                    }
                    case AgentState.Activity:
                    {
                        //TODO: implement when activities done
                        break;
                    }
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        private float3 ComputeNextTarget(ref SystemState state, in WorldTransform worldTransform,
            ref AgentComponent agent, ref DynamicBuffer<WorldActivityStepBufferElement> activityStepBuffer)
        {
            float3 target = worldTransform.Position;

            int wantedActivityId = activityStepBuffer[agent.NextActivityStepIndex].ActivityId;
            foreach (var (activityPosition, activityWorldTransform) in SystemAPI.Query<RefRW<ActivityPositionComponent>, WorldTransform>())
            {
                if (activityPosition.ValueRO.ActivityId == wantedActivityId)
                {
                    //TODO: reservation
                    //TODO: job Id, role Id (Guard), property Id (ex: beds)
                    target = activityWorldTransform.Position;
                    agent.NextIdleDuration = m_Random.NextFloat(activityPosition.ValueRO.IdleMinDuration, activityPosition.ValueRO.IdleMaxDuration);
                    break;
                }
            }

            agent.NextActivityStepIndex = (agent.NextActivityStepIndex + 1) % activityStepBuffer.Length;

            return target;
        }
    }
}
