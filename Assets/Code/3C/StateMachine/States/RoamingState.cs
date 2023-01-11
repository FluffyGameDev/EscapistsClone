using FluffyGameDev.Escapists.FSM;
using FluffyGameDev.Escapists.Input;
using UnityEngine;

namespace FluffyGameDev.Escapists.Player
{
    public class RoamingState : State
    {
        public override void OnEnter(StateMachineContext context)
        {
            GameObject player = context.Blackboard.Get<GameObject>((int)PlayerBB.PlayerGameObject);
            player.GetComponent<InputHandler>().enabled = true;
        }

        public override void OnExit(StateMachineContext context)
        {
            GameObject player = context.Blackboard.Get<GameObject>((int)PlayerBB.PlayerGameObject);
            player.GetComponent<InputHandler>().enabled = false;
        }
    }
}
