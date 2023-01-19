using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public abstract class HUDElementView
    {
        public VisualElement root { get; set; }

        public void Display()
        {
            if (root.style.display == DisplayStyle.None)
            {
                root.style.display = DisplayStyle.Flex;
                OnDisplay();
            }
        }

        public void Hide()
        {
            if (root.style.display == DisplayStyle.Flex)
            {
                root.style.display = DisplayStyle.None;
                OnHide();
            }
        }


        public virtual void Init() {}
        public virtual void Shutdown() {}
        protected virtual void OnDisplay() {}
        protected virtual void OnHide() {}
    }
}
