using FluffyGameDev.Escapists.Core;
using UnityEngine;

namespace FluffyGameDev.Escapists.Stats
{
    public interface IPlayerStatsService : IService
    {
        StatsContainer PlayerStatsContainer { get; }
    }

    public class PlayerStatsService : MonoBehaviour, IPlayerStatsService
    {
        private StatsContainer m_PlayerStatsContainer;

        private void Start()
        {
            ServiceLocator.RegisterService<IPlayerStatsService>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<IPlayerStatsService>();
        }

        public void Init()
        {
            m_PlayerStatsContainer = GetComponent<StatHolder>().Stats;
        }

        public void Shutdown()
        {
        }

        public StatsContainer PlayerStatsContainer => m_PlayerStatsContainer;
    }
}
