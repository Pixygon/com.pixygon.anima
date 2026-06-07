using System.Collections.Generic;
using UnityEngine;

namespace Pixygon.Anima {
    /// <summary>
    /// The set of <see cref="AnimaDefinition"/> species a game ships with, looked up by stable id.
    /// Resolves an owned <see cref="AnimaInstance"/> back to its species (e.g. to recompute stats or
    /// render its art).
    /// </summary>
    [CreateAssetMenu(menuName = "Pixygon/Anima/Anima Catalog", fileName = "AnimaCatalog")]
    public class AnimaCatalog : ScriptableObject {
        [SerializeField] private List<AnimaDefinition> _species = new();
        private Dictionary<int, AnimaDefinition> _byId;

        public IReadOnlyList<AnimaDefinition> Species => _species;

        public AnimaDefinition Get(int speciesId) {
            if (speciesId <= 0) return null;
            Build();
            return _byId.TryGetValue(speciesId, out var s) ? s : null;
        }

        /// <summary>Species of an owned instance (or null if not in this game's catalog).</summary>
        public AnimaDefinition SpeciesOf(AnimaInstance inst) => inst == null ? null : Get(inst.SpeciesId);

        private void Build() {
            if (_byId != null) return;
            _byId = new Dictionary<int, AnimaDefinition>();
            foreach (var s in _species)
                if (s != null) _byId[s.GetFullID] = s;
        }

        public void Invalidate() => _byId = null;

#if UNITY_EDITOR
        private void OnValidate() => Invalidate();
#endif
    }
}
