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
        private Direction m_FacingDirection = Direction.Down;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Interactor = GetComponent<Interactable.Interactor>();
        }

        private void Update()
        {
            var axis = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
            transform.position = transform.position + new Vector3(axis.x, axis.y, 0.0f) * m_Speed * Time.deltaTime;

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
            if (context.phase == InputActionPhase.Started)
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
            if (context.phase == InputActionPhase.Started)
            {
                m_WorldChannel.RaiseWorldInteractionRequest(transform.position, m_FacingDirection);
            }
        }
    }
}