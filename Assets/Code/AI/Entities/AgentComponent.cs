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
    }
}
