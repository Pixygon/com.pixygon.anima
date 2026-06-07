using Pixygon.Actors;
using UnityEngine;

namespace Pixygon.Anima {
    /// <summary>
    /// A creature <b>body</b> for an Actor (creatures = Actor + Anima). Implements <see cref="IBody"/>
    /// with <see cref="BodyKind.Creature"/> and renders the species' art. Minimal MVP — a 2D sprite or
    /// a 3D prefab; richer rigs/animation come later.
    /// </summary>
    public class AnimaBody : MonoBehaviour, IBody {
        [SerializeField] private AnimaDefinition _species;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public BodyKind Kind => BodyKind.Creature;
        public AnimaDefinition Species => _species;

        public void SetSpecies(AnimaDefinition species) {
            _species = species;
            Apply();
        }

        private void Apply() {
            if (_species == null) return;
            if (_spriteRenderer != null && _species._sprite != null)
                _spriteRenderer.sprite = _species._sprite;
            if (_species._bodyPrefab != null && transform.childCount == 0)
                Instantiate(_species._bodyPrefab, transform);
        }

        public void OnActorAttached(Actor actor) => Apply();
    }
}
