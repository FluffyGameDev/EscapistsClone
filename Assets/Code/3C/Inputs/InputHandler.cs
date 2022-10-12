using UnityEngine;
using UnityEngine.InputSystem;

namespace FluffyGameDev.Escapists.Input
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField]
        private float m_Speed = 1.0f;

        private PlayerInput m_PlayerInput;
        private Interactable.Interactor m_Interactor;

        private void Start()
        {
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Interactor = GetComponent<Interactable.Interactor>();
        }

        private void Update()
        {
            var axis = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
            transform.position = transform.position + new Vector3(axis.x, axis.y, 0.0f) * m_Speed * Time.deltaTime;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Interactable.Interactable bestInteractable = m_Interactor.bestInteractable;
            if (bestInteractable != null)
            {
                bestInteractable.TriggerInteraction();
            }
        }
    }
}