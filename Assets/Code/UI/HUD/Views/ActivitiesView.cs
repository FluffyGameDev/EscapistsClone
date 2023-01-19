using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.World;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class ActivitiesView : HUDElementView
    {
        private Label m_TimeLabel;
        private Label m_ActivityLabel;

        public override void Init()
        {
            m_TimeLabel = root.Q<Label>("lbl_Time");
            m_ActivityLabel = root.Q<Label>("lbl_Activity");
        }

        public void UpdateTime(DateTime time)
        {
            m_TimeLabel.text = $"Day {time.Day}: {time.Hour:D2}:{time.Minute:D2}";
        }

        public void UpdateActivity(Activity activity)
        {
            m_ActivityLabel.text = activity != null ? activity.ActivityName : "No Activity";
        }
    }
}
