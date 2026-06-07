using System;
using System.Collections.Generic;

namespace Pixygon.Anima {
    /// <summary>
    /// The "Pokédex" — per-species discovery record (seen / defeated / captured). Salvaged from
    /// PixygonMicro's <c>AnimaKnowledgeBase</c>, but **injectable** (pass an instance around) rather
    /// than a static singleton. Engine-portable.
    /// </summary>
    public sealed class AnimaBestiary {
        [Serializable]
        public sealed class Entry {
            public int speciesId;
            public bool seen;
            public int defeated;
            public int captured;
        }

        private readonly Dictionary<int, Entry> _entries = new();

        public Entry Of(int speciesId) {
            if (!_entries.TryGetValue(speciesId, out var e)) {
                e = new Entry { speciesId = speciesId };
                _entries[speciesId] = e;
            }
            return e;
        }

        public void See(int speciesId) => Of(speciesId).seen = true;
        public void Defeat(int speciesId) { var e = Of(speciesId); e.seen = true; e.defeated++; }
        public void Capture(int speciesId) { var e = Of(speciesId); e.seen = true; e.captured++; }

        public IEnumerable<Entry> All => _entries.Values;
        public int SeenCount { get { int n = 0; foreach (var e in _entries.Values) if (e.seen) n++; return n; } }
    }
}
