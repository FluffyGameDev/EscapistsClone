using FluffyGameDev.Escapists.Quest;

namespace FluffyGameDev.Escapists
{
    public abstract class GameplayEventQuestObjective : QuestObjective
    {
        private GameplayEvent m_GameplayEvent;

        public GameplayEventQuestObjective(GameplayEvent gameplayEvent)
        {
            m_GameplayEvent = gameplayEvent;
        }

        protected override void OnBeginQuestObjective()
        {
            m_GameplayEvent.OnEventRaised += OnGameplayEventRaised;
        }

        protected override void OnEndQuestObjective()
        {
            m_GameplayEvent.OnEventRaised -= OnGameplayEventRaised;
        }

        private void OnGameplayEventRaised()
        {
            if (IsGameplayEventValid())
            {
                NotifyQuestObjectiveCompleted();
            }
        }

        protected abstract bool IsGameplayEventValid();
    }

    public abstract class GameplayEventQuestObjective<T1> : QuestObjective
    {
        private GameplayEvent<T1> m_GameplayEvent;

        public GameplayEventQuestObjective(GameplayEvent<T1> gameplayEvent)
        {
            m_GameplayEvent = gameplayEvent;
        }

        protected override void OnBeginQuestObjective()
        {
            m_GameplayEvent.OnEventRaised += OnGameplayEventRaised;
        }

        protected override void OnEndQuestObjective()
        {
            m_GameplayEvent.OnEventRaised -= OnGameplayEventRaised;
        }

        private void OnGameplayEventRaised(T1 argument1)
        {
            if (IsGameplayEventValid(argument1))
            {
                NotifyQuestObjectiveCompleted();
            }
        }

        protected abstract bool IsGameplayEventValid(T1 argument1);
    }

    public abstract class GameplayEventQuestObjective<T1, T2> : QuestObjective
    {
        private GameplayEvent<T1, T2> m_GameplayEvent;

        public GameplayEventQuestObjective(GameplayEvent<T1, T2> gameplayEvent)
        {
            m_GameplayEvent = gameplayEvent;
        }

        protected override void OnBeginQuestObjective()
        {
            m_GameplayEvent.OnEventRaised += OnGameplayEventRaised;
        }

        protected override void OnEndQuestObjective()
        {
            m_GameplayEvent.OnEventRaised -= OnGameplayEventRaised;
        }

        private void OnGameplayEventRaised(T1 argument1, T2 argument2)
        {
            if (IsGameplayEventValid(argument1, argument2))
            {
                NotifyQuestObjectiveCompleted();
            }
        }

        protected abstract bool IsGameplayEventValid(T1 argument1, T2 argument2);
    }
}
