using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public enum AgentState
    {
        Idle,
        Walk,
        Combat,
        Activity
    }

    public struct AgentComponent : IComponentData
    {
        public AgentState State;
        public float IdleEndTime;
        public float NextIdleDuration;
        public int NextActivityStepIndex;
    }
}
