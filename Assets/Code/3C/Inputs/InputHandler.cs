using FluffyGameDev.Escapists.World;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FluffyGameDev.Escapists.Input
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField]
        private float m_Speed = 1.0f;
        [SerializeField]
        private WorldChannel m_WorldChannel;

        private PlayerInput m_PlayerInput;
        private Interactable.Interactor m_Interactor;
        private Animator m_Animator;
        private Rigidbody2D m_RigidBody;
        private Direction m_FacingDirection = Direction.Down;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_Interactor = GetComponent<Interactable.Interactor>();
        }

        private void Update()
        {
            var axis = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
            m_RigidBody.velocity = axis * m_Speed;

            if (axis != Vector2.zero)
            {
                m_FacingDirection = ComputeDirectionFromMovement(axis);
            }

            m_Animator.SetFloat("WalkingSpeed", axis.magnitude);
            m_Animator.SetInteger("FacingDirection", (int)m_FacingDirection);
        }

        private Direction ComputeDirectionFromMovement(Vector2 movement)
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

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (enabled && context.phase == InputActionPhase.Started)
            {
                Interactable.Interactable bestInteractable = m_Interactor.bestInteractable;
                if (bestInteractable != null)
                {
                    bestInteractable.TriggerInteraction();
                }
            }
        }

        public void OnWorldInteract(InputAction.CallbackContext context)
        {
            if (enabled && context.phase == InputActionPhase.Started)
            {
                m_WorldChannel.RaiseWorldInteractionRequest(transform.position, m_FacingDirection);
            }
        }
    }
}