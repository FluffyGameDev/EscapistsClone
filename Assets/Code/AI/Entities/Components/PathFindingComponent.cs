using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace FluffyGameDev.Escapists.AI
{
    public enum PathFindingState : byte
    {
        Idle,
        Searching,
        Moving,
        AtDestination
    }

    public struct PathFindingComponent : IComponentData
    {
        public NativeList<int2> Path;
        public float3 TargetPosition;
        public int CurrentStepIndex;
        public PathFindingState State;

        public void StartPathFinding(float3 targetPosition)
        {
            TargetPosition = targetPosition;
            Path.Clear();
            CurrentStepIndex = -1;
            State = PathFindingState.Searching;
        }
    }
}
