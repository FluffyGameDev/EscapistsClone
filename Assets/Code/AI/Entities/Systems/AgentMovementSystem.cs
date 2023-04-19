using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace FluffyGameDev.Escapists.AI
{
    [BurstCompile]
    public partial struct AgentMovementSystem : ISystem
    {
        private const float k_TargetDistanceEpsilon = 0.0001f;

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
            //TODO: move to a job
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (worldTransform, movementTarget, agentEntity) in SystemAPI.Query<RefRW<WorldTransform>, MovementTargetComponent>().WithEntityAccess())
            {
                float3 movement = movementTarget.TargetPosition - worldTransform.ValueRO.Position;
                float squareDistance = math.lengthsq(movement);
                if (squareDistance >= k_TargetDistanceEpsilon)
                {
                    float movementDistance = movementTarget.Speed * Time.deltaTime;
                    if (squareDistance < movementDistance * movementDistance)
                    {
                        worldTransform.ValueRW.Position = movementTarget.TargetPosition;
                        ecb.SetComponentEnabled<MovementTargetComponent>(agentEntity, false);
                    }
                    else
                    {
                        worldTransform.ValueRW.Position += math.normalize(movement) * movementDistance;
                    }
                }
                else
                {
                    ecb.SetComponentEnabled<MovementTargetComponent>(agentEntity, false);
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
