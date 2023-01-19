using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class ToolView : HUDElementView
    {
        private VisualElement m_ToolProgressContainer;
        private ProgressBar m_ToolProgressBar;

        public override void Init()
        {
            m_ToolProgressContainer = root.Q<VisualElement>("ctr_tool");
            m_ToolProgressBar = root.Q<ProgressBar>("pb_ToolProgress");
        }

        public void UpdateToolUseBar(bool isUsingTool, float progress)
        {
            m_ToolProgressContainer.style.display = isUsingTool ? DisplayStyle.Flex : DisplayStyle.None;

            if (isUsingTool)
            {
                m_ToolProgressBar.value = progress;
            }
        }
    }
}
