using System;

namespace FluffyGameDev.Escapists.FSM
{
    public class CheckBlackBoardTransition<T> : Transition
        where T : IComparable
    {
        private int m_BBEntryID;
        private T m_ExpectedValue;

        public CheckBlackBoardTransition(int sourceStateID, int destinationStateID, int bbEntryID, T expectedValue)
            : base(sourceStateID, destinationStateID)
        {
            m_BBEntryID = bbEntryID;
            m_ExpectedValue = expectedValue;
        }

        public override bool CanPerformTransition(StateMachineContext context)
        {
            return context.Blackboard.Get<T>(m_BBEntryID).CompareTo(m_ExpectedValue) == 0;
        }
    }
}
