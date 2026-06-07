using System.Collections.Generic;
using Pixygon.ID;
using UnityEngine;

namespace Pixygon.Anima {
    /// <summary>
    /// A creature <b>species</b> template (salvaged from PixygonMicro's <c>AnimaCreatureData</c>): base
    /// stats, type, art, move pool, rewards. Identified by <see cref="IdObject"/> so the same anima is
    /// the same entity across every game (and the wiki). <see cref="MakeInstance"/> spawns a fresh
    /// owned creature.
    /// </summary>
    [CreateAssetMenu(menuName = "Pixygon/Anima/Anima Definition", fileName = "Anima")]
    public class AnimaDefinition : IdObject {
        public string _displayName;
        [TextArea] public string _description;

        public AnimaType _type;
        public AnimaStatLine _baseStats;

        [Header("Art")]
        public Sprite _sprite;
        public Sprite _portrait;
        public GameObject _bodyPrefab;   // 3D body (optional; 2D games use _sprite)

        [Header("Progression")]
        public int _xpReward = 23;

        [System.Serializable]
        public struct MoveUnlock {
            public AnimaMove move;
            public int levelUnlock;
        }
        public List<MoveUnlock> _movePool = new();

        /// <summary>Build a fresh, account-ownable instance of this species at a level.</summary>
        public AnimaInstance MakeInstance(int level = 1, Nature nature = Nature.Neutral) {
            var inst = new AnimaInstance {
                SpeciesId = GetFullID,
                Nickname = _displayName,
                Nature = nature,
                Level = Mathf.Max(1, level),
            };
            foreach (var m in _movePool)
                if (m.move != null && m.levelUnlock <= inst.Level)
                    inst.LearnedMoveIds.Add(m.move.GetFullID);
            inst.FillVitals(_baseStats);
            return inst;
        }
    }
}
