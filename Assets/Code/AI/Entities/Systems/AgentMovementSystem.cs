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

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //TODO: move to a job
            foreach (var (worldTransform, movementTarget, agentEntity) in SystemAPI.Query<RefRW<WorldTransform>, RefRW<MovementTargetComponent>>().WithEntityAccess())
            {
                if (movementTarget.ValueRW.IsActive)
                {
                    float3 movement = movementTarget.ValueRO.TargetPosition - worldTransform.ValueRO.Position;
                    float squareDistance = math.lengthsq(movement);
                    if (squareDistance >= k_TargetDistanceEpsilon)
                    {
                        float movementDistance = movementTarget.ValueRO.Speed * Time.deltaTime;
                        if (squareDistance < movementDistance * movementDistance)
                        {
                            worldTransform.ValueRW.Position = movementTarget.ValueRO.TargetPosition;
                            movementTarget.ValueRW.IsActive = false;
                        }
                        else
                        {
                            worldTransform.ValueRW.Position += math.normalize(movement) * movementDistance;
                        }
                    }
                    else
                    {
                        movementTarget.ValueRW.IsActive = false;
                    }
                }
            }
        }
    }
}
