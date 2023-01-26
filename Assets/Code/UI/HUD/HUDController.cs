using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField]
        private UIDocument m_UIDocument;

        private VisualElement m_Root;
        private List<IHUDElementPresenter> m_HUDElements = new();

        private void Awake()
        {
            m_Root = m_UIDocument.rootVisualElement;
        }

        private void OnDestroy()
        {
            m_HUDElements.ForEach(element => element.Shutdown());
        }

        public VisualElement FindElement(string viewElementId)
        {
            return m_Root.Q(viewElementId);
        }

        public void RegisterHUDElement(IHUDElementPresenter presenter)
        {
            presenter.Init();
            m_HUDElements.Add(presenter);
        }
    }
}
