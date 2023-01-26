using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    public abstract class HUDElementPresenter<T> : MonoBehaviour, IHUDElementPresenter
        where T : HUDElementView, new()
    {
        [SerializeField]
        private HUDController m_HUDController;
        [SerializeField]
        private string m_RootElementName;

        public T view { get; set; }

        private void Start()
        {
            view = new();
            view.root = m_HUDController.FindElement(m_RootElementName);
            m_HUDController.RegisterHUDElement(this);
        }

        public void Init()
        {
            view.Init();
            OnInit();
        }

        public void Shutdown()
        {
            OnShutdown();
            view.Shutdown();
        }

        protected virtual void OnInit() { }
        protected virtual void OnShutdown() { }
    }

    public interface IHUDElementPresenter
    {
        void Init();
        void Shutdown();
    }
}
