using FluffyGameDev.Escapists.World;
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
            DynamicBuffer<WorldActivityStepBufferElement> activityStepBuffer = SystemAPI.GetSingletonBuffer<WorldActivityStepBufferElement>();

            foreach (var (agent, movementTarget, worldTransform, agentEntity) in
                SystemAPI.Query<RefRW<AgentComponent>, RefRW<MovementTargetComponent>, WorldTransform>()
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
                            movementTarget.ValueRW.TargetPosition = ComputeNextTarget(ref state, in worldTransform, agentEntity, ref agent.ValueRW, ref activityStepBuffer);
                            movementTarget.ValueRW.IsActive = true;
                        }
                        break;
                    }
                    case AgentState.Walk:
                    {
                        // TODO: switch to path finding component when implemented
                        if (!movementTarget.ValueRO.IsActive)
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
                }
            }
        }

        private float3 ComputeNextTarget(ref SystemState state, in WorldTransform worldTransform, Entity agentEntity,
            ref AgentComponent agent, ref DynamicBuffer<WorldActivityStepBufferElement> activityStepBuffer)
        {
            float3 target = worldTransform.Position;

            int wantedActivityId = activityStepBuffer[agent.NextActivityStepIndex].ActivityId;
            foreach (var (activityPosition, activityWorldTransform) in SystemAPI.Query<RefRW<ActivityPositionComponent>, WorldTransform>())
            {
                if (activityPosition.ValueRO.ActivityId == wantedActivityId &&
                    (activityPosition.ValueRO.OwnerAgentId == AgentComponent.InvalidAgentId || activityPosition.ValueRO.OwnerAgentId == agent.AgentId) &&
                    (!activityPosition.ValueRO.CanBeReserved || activityPosition.ValueRO.ReservingEntity == Entity.Null) &&
                    (activityPosition.ValueRO.AgentJobId == AgentJobIdentifier.InvalidJobId || activityPosition.ValueRO.AgentJobId == agent.AgentJobId) &&
                    (activityPosition.ValueRO.AgentRoleId == AgentRoleIdentifier.InvalidRoleId || activityPosition.ValueRO.AgentRoleId == agent.AgentRoleId))
                {
                    if (activityPosition.ValueRO.CanBeReserved)
                    {
                        activityPosition.ValueRW.ReservingEntity = agentEntity;
                    }

                    target = activityWorldTransform.Position;
                    agent.NextIdleDuration = m_Random.NextFloat(activityPosition.ValueRO.IdleMinDuration, activityPosition.ValueRO.IdleMaxDuration);
                    break;
                }
                else if (activityPosition.ValueRO.ReservingEntity == agentEntity)
                {
                    activityPosition.ValueRW.ReservingEntity = Entity.Null;
                }
            }

            agent.NextActivityStepIndex = (agent.NextActivityStepIndex + 1) % activityStepBuffer.Length;

            return target;
        }
    }
}
