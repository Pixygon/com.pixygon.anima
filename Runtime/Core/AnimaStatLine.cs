using System;

namespace Pixygon.Anima {
    /// <summary>
    /// The seven creature stats (salvaged from PixygonMicro's <c>AnimaStats</c>). Engine-portable.
    ///
    /// Mapping onto the shared stat catalog (`com.pixygon.stats`), when an anima drives an Actor's
    /// StatBlock: Vitality→Health, PhysicalAttack→PhysicalPower, SpecialAttack→MagicPower,
    /// PhysicalDefence→Defense, SpecialDefence→MagicDefense, Speed→Agility. Poise (break-stun) stays
    /// anima-internal.
    /// </summary>
    [Serializable]
    public struct AnimaStatLine {
        public int PhysicalAttack;
        public int SpecialAttack;
        public int PhysicalDefence;
        public int SpecialDefence;
        public int Speed;
        public int Vitality;   // the HP base
        public int Poise;      // break-stun threshold

        /// <summary>Pokémon-style growth: <c>floor(level * (base/50 + trainingValue) * natureMult)</c>.</summary>
        public static int Grow(int baseStat, int trainingValue, int level, float natureMult) =>
            (int)Math.Floor(level * (baseStat / 50f + trainingValue) * natureMult);
    }

    /// <summary>Nature → stat multiplier.</summary>
    public static class NatureRules {
        public static float Multiplier(Nature n, bool offensive) => n switch {
            Nature.Aggressive => offensive ? 1.1f : 1f,
            Nature.Defensive  => offensive ? 1f : 1.1f,
            _ => 1f,
        };
    }
}
