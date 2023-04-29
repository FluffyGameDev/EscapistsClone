using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public enum AgentState
    {
        Idle,
        Walk,
        Combat
    }

    public struct AgentComponent : IComponentData
    {
        public const int InvalidAgentId = -1;

        public int AgentId;
        public int AgentJobId;
        public int AgentRoleId;
        public AgentState State;
        public float IdleEndTime;
        public float NextIdleDuration;
        public int NextActivityStepIndex;
    }
}
