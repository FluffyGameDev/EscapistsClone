
namespace FluffyGameDev.Escapists.FSM
{
    public abstract class State
    {
        public virtual void OnEnter() {}
        public virtual void OnExit() {}
        public virtual void OnUpdate(StateMachineContext context, float dt) {}
    }
}
