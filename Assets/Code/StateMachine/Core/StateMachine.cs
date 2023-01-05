using System.Collections.Generic;

namespace FluffyGameDev.Escapists.FSM
{
    public class StateMachine
    {
        private Dictionary<int, State> m_States = new();
        private List<Transition> m_Transitions = new();

        public void RegisterState(int id, State state)
        {
            m_States[id] = state;
        }

        public void RegisterTransition(Transition transition)
        {
            m_Transitions.Add(transition);
        }

        public void UpdateStateMachine(StateMachineContext context, float dt)
        {
            State currentState;
            m_States.TryGetValue(context.CurrentStateID, out currentState);

            foreach (Transition transition in m_Transitions)
            {
                if (transition.SourceStateID == context.CurrentStateID && transition.CanPerformTransition(context))
                {
                    currentState?.OnExit();

                    context.CurrentStateID = transition.DestinationStateID;
                    m_States.TryGetValue(context.CurrentStateID, out currentState);

                    currentState?.OnEnter();
                    break;
                }
            }

            currentState?.OnUpdate(context, dt);
        }

    }
}
