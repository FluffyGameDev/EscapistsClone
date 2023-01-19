using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    public abstract class HUDElementPresenter<T> : MonoBehaviour, IHUDElementPresenter
        where T : HUDElementView
    {
        public T view { get; set; }

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
