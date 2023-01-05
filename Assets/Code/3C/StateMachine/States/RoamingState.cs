using FluffyGameDev.Escapists.FSM;
using UnityEngine;

namespace FluffyGameDev.Escapists.Player
{
    public class RoamingState : State
    {
        public override void OnEnter()
        {
            Debug.Log("RoamingState.OnEnter");
        }

        public override void OnExit()
        {
            Debug.Log("RoamingState.OnExit");
        }

        public override void OnUpdate(StateMachineContext context, float dt)
        {
            Debug.Log("RoamingState.OnUpdate");
        }
    }
}
