using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FluffyGameDev.Escapists.Interactable
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent m_OnInteract;

        private List<Interactor> m_Interactors = new();

        public void TriggerInteraction()
        {
            m_OnInteract?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Interactor interactor = collision.gameObject.GetComponent<Interactor>();
            if (interactor != null)
            {
                interactor.RegisterPossibleInteractable(this);
                m_Interactors.Add(interactor);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Interactor interactor = collision.gameObject.GetComponent<Interactor>();
            if (interactor != null)
            {
                interactor.UnregisterPossibleInteractable(this);
                m_Interactors.Remove(interactor);
            }
        }

        private void OnDisable()
        {
            foreach (Interactor interactor in m_Interactors)
            {
                interactor.UnregisterPossibleInteractable(this);
                m_Interactors.Remove(interactor);
            }
        }
    }
}