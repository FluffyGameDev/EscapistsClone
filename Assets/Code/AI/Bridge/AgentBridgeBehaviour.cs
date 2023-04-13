using FluffyGameDev.Escapists.World;
using UnityEngine;

namespace FluffyGameDev.Escapists.AI
{
    public class AgentBridgeBehaviour : MonoBehaviour
    {
        private AgentState m_State;
        private Direction m_FacingDirection = Direction.Down;
        private Animator m_Animator;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        public void UpdateBridgeData(AgentState state, Vector3 position)
        {
            m_State = state;

            Vector3 movement = (position - transform.position) / Time.deltaTime;
            transform.position = position;

            if (movement != Vector3.zero)
            {
                m_FacingDirection = ComputeDirectionFromMovement(movement);
            }

            m_Animator.SetFloat("WalkingSpeed", movement.magnitude);
            m_Animator.SetInteger("FacingDirection", (int)m_FacingDirection);
        }

        //TODO: move to utils
        private Direction ComputeDirectionFromMovement(Vector3 movement)
        {
            Direction direction;
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                direction = movement.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                direction = movement.y > 0 ? Direction.Up : Direction.Down;
            }
            return direction;
        }
    }
}
