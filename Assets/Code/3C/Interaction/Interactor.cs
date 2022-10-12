using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Escapists.Interactable
{
    public class Interactor : MonoBehaviour
    {
        private List<Interactable> m_NearbyInteractables = new();

        public Interactable bestInteractable
        {
            get
            {
                //TODO: add ponderation system.
                return m_NearbyInteractables.Count > 0 ? m_NearbyInteractables[0] : null;
            }
        }

        public void RegisterPossibleInteractable(Interactable interactable)
        {
            m_NearbyInteractables.Add(interactable);
        }

        public void UnregisterPossibleInteractable(Interactable interactable)
        {
            m_NearbyInteractables.Remove(interactable);
        }
    }
}
