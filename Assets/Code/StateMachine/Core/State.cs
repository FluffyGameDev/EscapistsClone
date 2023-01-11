
namespace FluffyGameDev.Escapists.FSM
{
    public abstract class State
    {
        public virtual void OnEnter(StateMachineContext context) {}
        public virtual void OnExit(StateMachineContext context) {}
        public virtual void OnUpdate(StateMachineContext context, float dt) {}
    }
}
