using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FluffyGameDev.Escapists.UI
{
    public class StatsView : HUDElementView
    {
        private List<StatUIPresenter> m_StatPresenters = new();

        public override void Shutdown()
        {
            m_StatPresenters.ForEach(x => x.Shutdown());
            m_StatPresenters.Clear();
        }

        public void Setup(Stats.StatsContainer statContainer, Stats.StatDescriptor healthStat, Stats.StatDescriptor staminaStat, Stats.StatDescriptor heatStat)
        {
            m_StatPresenters.Add(new StatUIPresenter(root.Q<Label>("lbl_HealthValue"), statContainer, healthStat));
            m_StatPresenters.Add(new StatUIPresenter(root.Q<Label>("lbl_StaminaValue"), statContainer, staminaStat));
            m_StatPresenters.Add(new StatUIPresenter(root.Q<Label>("lbl_HeatValue"), statContainer, heatStat));
        }


        private class StatUIPresenter
        {
            private Label m_ValueLabel;
            private Stats.Stat m_Stat;

            public StatUIPresenter(Label valueLabel, Stats.StatsContainer statsContainer, Stats.StatData statDescriptor)
            {
                m_ValueLabel = valueLabel;
                m_Stat = statsContainer.GetStat(statDescriptor);

                RefreshStatValue(m_Stat);
                m_Stat.OnStatChanged += RefreshStatValue;
            }

            public void Shutdown()
            {
                m_Stat.OnStatChanged -= RefreshStatValue;
            }

            private void RefreshStatValue(Stats.Stat stat)
            {
                switch (stat.StatType)
                {
                    case Stats.StatType.Integer:
                        {
                            m_ValueLabel.text = stat.GetValueInt().ToString();
                            break;
                        }

                    case Stats.StatType.Float:
                        {
                            m_ValueLabel.text = $"{(int)(stat.GetValueFloat() * 100.0f)} %";
                            break;
                        }
                }
            }
        }
    }
}
