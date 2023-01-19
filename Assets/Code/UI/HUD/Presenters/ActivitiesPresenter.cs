using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.World;

namespace FluffyGameDev.Escapists.UI
{
    public class ActivitiesPresenter : HUDElementPresenter<ActivitiesView>
    {
        protected override void OnInit()
        {
            ServiceLocator.WaitUntilReady<ITimeService>(InitTimeUI);
            ServiceLocator.WaitUntilReady<IScheduleService>(InitActivityUI);
        }

        private void InitTimeUI()
        {
            ITimeService timeService = ServiceLocator.LocateService<ITimeService>();
            view.UpdateTime(timeService.CurrentTime);
            timeService.OnTimeChanges += view.UpdateTime;
        }

        private void InitActivityUI()
        {
            IScheduleService scheduleService = ServiceLocator.LocateService<IScheduleService>();
            view.UpdateActivity(scheduleService.CurrentActivity);
            scheduleService.OnActivityChange += view.UpdateActivity;
        }
    }
}
