using System;
using System.Collections.Generic;

namespace FluffyGameDev.Escapists
{
    public class Blackboard
    {
        private class BlackboardEntry
        {
            public object Value;
            public Action<int> OnValueChange;
        }

        private List<BlackboardEntry> m_Entries;

        public Blackboard(int entryCount)
        {
            m_Entries = new(entryCount);
            for (int i = 0; i < entryCount; ++i)
            {
                m_Entries.Add(new BlackboardEntry());
            }
        }

        public EntryValue Get<EntryValue>(int id)
        {
            return (EntryValue)m_Entries[id].Value;
        }

        public void Set<EntryValue>(int id, EntryValue value)
        {
            BlackboardEntry entry = m_Entries[id];
            entry.Value = value;
            entry.OnValueChange?.Invoke(id);
        }

        public void RegisterEntryChangedCallback(int id, Action<int> callback)
        {
            m_Entries[id].OnValueChange += callback;
        }

        public void UnregisterEntryChangedCallback(int id, Action<int> callback)
        {
            m_Entries[id].OnValueChange -= callback;
        }
    }
}
