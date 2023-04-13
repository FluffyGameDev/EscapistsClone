using FluffyGameDev.Escapists.Core;
using FluffyGameDev.Escapists.World;
using Unity.Entities;

namespace FluffyGameDev.Escapists.AI
{
    public partial struct WorldActivityUpdaterSystem : ISystem
    {
        private int m_CurrentActivityStartTime;

        public void OnCreate(ref SystemState state)
        {
            state.EntityManager.CreateSingletonBuffer<WorldActivityStepBufferElement>();
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            IScheduleService scheduleService = ServiceLocator.LocateService<IScheduleService>();
            Activity currentActivity = scheduleService.CurrentActivity;
            if (currentActivity != null && m_CurrentActivityStartTime != currentActivity.StartTime)
            {
                m_CurrentActivityStartTime = currentActivity.StartTime;
                DynamicBuffer<WorldActivityStepBufferElement> activityStepBuffer = SystemAPI.GetSingletonBuffer<WorldActivityStepBufferElement>();
                activityStepBuffer.Clear();
                foreach (var activityIdentifier in currentActivity.ActivityIdentifiers)
                {
                    activityStepBuffer.Add(new WorldActivityStepBufferElement() { ActivityId = activityIdentifier.ActivityId });
                }

                foreach (var agent in SystemAPI.Query<RefRW<AgentComponent>>())
                {
                    agent.ValueRW.NextActivityStepIndex = 0;
                    agent.ValueRW.IdleEndTime = 0.0f;
                }
            }
        }
    }
}
