using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 1.0f;

    private PlayerInput m_PlayerInput;

    private void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        var axis = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
        transform.position = transform.position + new Vector3(axis.x, axis.y, 0.0f) * m_Speed * Time.fixedDeltaTime;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }
}
