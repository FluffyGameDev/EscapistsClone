using FluffyGameDev.Escapists.FSM;
using UnityEngine;

namespace FluffyGameDev.Escapists.Player
{
    public class ToolState : State
    {
        private const float AnimationDuration = 2.5f;

        private PlayerChannel m_PlayerChannel;

        public ToolState(PlayerChannel playerChannel)
        {
            m_PlayerChannel = playerChannel;
        }

        public override void OnEnter(StateMachineContext context)
        {
            context.Blackboard.Set((int)PlayerBB.ToolUseStartTime, Time.time);
        }

        public override void OnExit(StateMachineContext context)
        {
            m_PlayerChannel.RaiseToolUseSucceeded();
        }

        public override void OnUpdate(StateMachineContext context, float dt)
        {
            float startTime = context.Blackboard.Get<float>((int)PlayerBB.ToolUseStartTime);
            if (Time.time >= startTime + AnimationDuration)
            {
                context.Blackboard.Set((int)PlayerBB.IsUsingTool, false);
            }
        }
    }
}
