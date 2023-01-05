using FluffyGameDev.Escapists.FSM;
using UnityEngine;

namespace FluffyGameDev.Escapists.Player
{
    public class ToolState : State
    {
        public override void OnEnter()
        {
            Debug.Log("ToolState.OnEnter");
        }

        public override void OnExit()
        {
            Debug.Log("ToolState.OnExit");
        }

        public override void OnUpdate(StateMachineContext context, float dt)
        {
            Debug.Log("ToolState.OnUpdate");
        }
    }
}
