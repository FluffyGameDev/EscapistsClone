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
            context.Blackboard.Set((int)PlayerBB.ToolUseEndTime, Time.time + AnimationDuration);
        }

        public override void OnExit(StateMachineContext context)
        {
            m_PlayerChannel.RaiseToolUseSucceeded();
        }

        public override void OnUpdate(StateMachineContext context, float dt)
        {
            float endTime = context.Blackboard.Get<float>((int)PlayerBB.ToolUseEndTime);
            if (Time.time >= endTime)
            {
                context.Blackboard.Set((int)PlayerBB.IsUsingTool, false);
            }
        }
    }
}
