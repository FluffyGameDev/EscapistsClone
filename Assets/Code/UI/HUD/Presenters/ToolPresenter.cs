using FluffyGameDev.Escapists.Player;
using UnityEngine;

namespace FluffyGameDev.Escapists.UI
{
    public class ToolPresenter : HUDElementPresenter<ToolView>
    {
        [SerializeField]
        private PlayerStateMachineHolder m_PlayerStateMachineHolder;

        private void Update()
        {
            UpdateToolUseBar();
        }

        private void UpdateToolUseBar()
        {
            float progress = 0.0f;
            bool isUsingTool = m_PlayerStateMachineHolder.blackboard.Get<bool>((int)PlayerBB.IsUsingTool);

            if (isUsingTool)
            {
                float startTime = m_PlayerStateMachineHolder.blackboard.Get<float>((int)PlayerBB.ToolUseStartTime);
                float endTime = m_PlayerStateMachineHolder.blackboard.Get<float>((int)PlayerBB.ToolUseEndTime);

                float duration = endTime - startTime;
                progress = (Time.time - startTime) / duration;
            }

            view.UpdateToolUseBar(isUsingTool, progress);
        }
    }
}
