using System;
using System.Runtime.InteropServices;

namespace FluffyGameDev.Escapists.Stats
{
    [StructLayout(LayoutKind.Explicit)]
    public struct StatValue
    {
        [FieldOffset(0)]
        public int IntValue;
        [FieldOffset(0)]
        public float FloatValue;
    }

    public enum StatType
    {
        Integer,
        Float
    }

    public class Stat
    {
        private StatType m_StatType;
        private StatValue m_Value;

        public StatType StatType => m_StatType;
        public event Action<Stat> OnStatChanged;

        public Stat(int value)
        {
            m_StatType = StatType.Integer;
            m_Value.IntValue = value;
        }

        public Stat(float value)
        {
            m_StatType = StatType.Float;
            m_Value.FloatValue = value;
        }

        public int GetValueInt()
        {
            if (m_StatType == StatType.Integer) //TODO: ifdef debug
            {
                return m_Value.IntValue;
            }
            else
            {
                //TODO: error
                return 0;
            }
        }

        public float GetValueFloat()
        {
            if (m_StatType == StatType.Float) //TODO: ifdef debug
            {
                return m_Value.FloatValue;
            }
            else
            {
                //TODO: error
                return 0.0f;
            }
        }

        public void SetValue(int value)
        {
            if (m_StatType == StatType.Integer) //TODO: ifdef debug
            {
                m_Value.IntValue = value;
                OnStatChanged?.Invoke(this);
            }
            else
            {
                //TODO: error
            }
        }

        public void SetValue(float value)
        {
            if (m_StatType == StatType.Float) //TODO: ifdef debug
            {
                m_Value.FloatValue = value;
                OnStatChanged?.Invoke(this);
            }
            else
            {
                //TODO: error
            }
        }
    }
}
