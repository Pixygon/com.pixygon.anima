using System;
using System.Collections.Generic;

namespace Pixygon.Anima {
    /// <summary>
    /// An <b>owned creature instance</b> — YOURS, account-level, portable across games. Engine-portable
    /// (no UnityEngine), so it serializes into the account ItemBox and any game's save. References its
    /// species by stable id; effective stats are computed from the species base + level + EVs + nature.
    /// </summary>
    [Serializable]
    public sealed class AnimaInstance {
        public string Id;          // unique instance id (assigned by the vault/game)
        public int SpeciesId;      // AnimaDefinition.GetFullID
        public string Nickname;
        public Nature Nature;

        public int Level = 1;
        public int CurrentXp;
        public int CurrentHp;
        public int CurrentAp;      // ability points (special-move resource)
        public int CurrentPoise;

        public AnimaStatLine TrainingValues; // EVs — permanent growth
        public AnimaAilment Ailment;
        public List<int> LearnedMoveIds = new();

        /// <summary>Safe in account storage, or deployed (at risk) in a game. The permadeath stake.</summary>
        public AnimaLocation Location = AnimaLocation.AccountStorage;

        public bool IsFainted => CurrentHp <= 0;

        /// <summary>Effective stats: grow the species base by level / EVs / nature.</summary>
        public AnimaStatLine EffectiveStats(AnimaStatLine baseStats) {
            float off = NatureRules.Multiplier(Nature, true);
            float def = NatureRules.Multiplier(Nature, false);
            return new AnimaStatLine {
                PhysicalAttack  = AnimaStatLine.Grow(baseStats.PhysicalAttack,  TrainingValues.PhysicalAttack,  Level, off),
                SpecialAttack   = AnimaStatLine.Grow(baseStats.SpecialAttack,   TrainingValues.SpecialAttack,   Level, off),
                PhysicalDefence = AnimaStatLine.Grow(baseStats.PhysicalDefence, TrainingValues.PhysicalDefence, Level, def),
                SpecialDefence  = AnimaStatLine.Grow(baseStats.SpecialDefence,  TrainingValues.SpecialDefence,  Level, def),
                Speed           = AnimaStatLine.Grow(baseStats.Speed,           TrainingValues.Speed,           Level, 1f),
                Vitality        = AnimaStatLine.Grow(baseStats.Vitality,        TrainingValues.Vitality,        Level, 1f),
                Poise           = AnimaStatLine.Grow(baseStats.Poise,           TrainingValues.Poise,           Level, 1f),
            };
        }

        /// <summary>Restore HP/AP/Poise to their effective maxima (on heal / fresh capture).</summary>
        public void FillVitals(AnimaStatLine baseStats) {
            var s = EffectiveStats(baseStats);
            CurrentHp = s.Vitality;
            CurrentPoise = s.Poise;
        }
    }
}
