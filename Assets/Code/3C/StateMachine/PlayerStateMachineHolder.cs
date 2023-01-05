using UnityEngine;
using FluffyGameDev.Escapists.FSM;

namespace FluffyGameDev.Escapists.Player
{
    public class PlayerStateMachineHolder : MonoBehaviour
    {
        public enum PlayerState
        {
            Roaming,
            UsingTool,
            Down
        }

        //TODO: remove
        [SerializeField]
        private int m_Health = 10;
        [SerializeField]
        private bool m_IsUsingTool = false;
        [SerializeField]
        private PlayerState m_CurrentState;
        ////////////////


        private StateMachine m_StateMachine = new();
        private StateMachineContext m_StateMachineContext = new();

        private void Awake()
        {
            InitStateMachine();
        }

        private void Update()
        {
            m_StateMachine.UpdateStateMachine(m_StateMachineContext, Time.deltaTime);

            //TODO: remove
            m_StateMachineContext.Blackboard.Set((int)PlayerBB.Health, m_Health);
            m_StateMachineContext.Blackboard.Set((int)PlayerBB.IsUsingTool, m_IsUsingTool);
            m_CurrentState = (PlayerState)m_StateMachineContext.CurrentStateID;
            ///////////
        }


        private void InitStateMachine()
        {
            Blackboard playerBB = new((int)PlayerBB.Count);
            playerBB.Set((int)PlayerBB.PlayerGameObject, gameObject);
            playerBB.Set((int)PlayerBB.Health, 10);
            playerBB.Set((int)PlayerBB.IsUsingTool, false);
            m_StateMachineContext.Blackboard = playerBB;

            m_StateMachine.RegisterState((int)PlayerState.Roaming, new RoamingState());
            m_StateMachine.RegisterState((int)PlayerState.UsingTool, new ToolState());
            m_StateMachine.RegisterState((int)PlayerState.Down, new DownState());

            //TODO: remove debug transition
            m_StateMachine.RegisterTransition(new CheckBlackBoardTransition<int>(-1, (int)PlayerState.Roaming, (int)PlayerBB.Health, 10));

            m_StateMachine.RegisterTransition(new CheckBlackBoardTransition<bool>((int)PlayerState.Roaming, (int)PlayerState.UsingTool, (int)PlayerBB.IsUsingTool, true));
            m_StateMachine.RegisterTransition(new CheckBlackBoardTransition<bool>((int)PlayerState.UsingTool, (int)PlayerState.Roaming, (int)PlayerBB.IsUsingTool, false));
            m_StateMachine.RegisterTransition(new CheckBlackBoardTransition<int>((int)PlayerState.Roaming, (int)PlayerState.Down, (int)PlayerBB.Health, 0));
            m_StateMachine.RegisterTransition(new CheckBlackBoardTransition<int>((int)PlayerState.UsingTool, (int)PlayerState.Down, (int)PlayerBB.Health, 0));
        }
    }
}
