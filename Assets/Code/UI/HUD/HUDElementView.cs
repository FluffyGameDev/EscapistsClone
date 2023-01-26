using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public abstract class HUDElementView
    {
        public VisualElement root { get; set; }

        public void Display()
        {
            root.style.display = DisplayStyle.Flex;
            root.style.visibility = Visibility.Visible;
            OnDisplay();
        }

        public void Hide()
        {
            root.style.display = DisplayStyle.None;
            root.style.visibility = Visibility.Hidden;
            OnHide();
        }


        public virtual void Init() {}
        public virtual void Shutdown() {}
        protected virtual void OnDisplay() {}
        protected virtual void OnHide() {}
    }
}
