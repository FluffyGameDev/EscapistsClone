
namespace FluffyGameDev.Escapists.FSM
{
    public abstract class Transition
    {
        public int SourceStateID { get; }
        public int DestinationStateID { get; }

        public Transition(int sourceStateID, int destinationStateID)
        {
            SourceStateID = sourceStateID;
            DestinationStateID = destinationStateID;
        }

        public abstract bool CanPerformTransition(StateMachineContext context);
    }
}
