using FluffyGameDev.Escapists.FSM;
using UnityEngine;

namespace FluffyGameDev.Escapists.Player
{
    public class DownState : State
    {
        public override void OnEnter()
        {
            Debug.Log("DownState.OnEnter");
        }

        public override void OnExit()
        {
            Debug.Log("DownState.OnExit");
        }

        public override void OnUpdate(StateMachineContext context, float dt)
        {
            Debug.Log("DownState.OnUpdate");
        }
    }
}
