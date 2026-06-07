namespace Pixygon.Anima {
    // ── Engine-portable core (no UnityEngine). ──

    /// <summary>Creature type, used for move/affinity effectiveness. The first five align with the
    /// universe elements (Fire/Earth/Water/Air/Aether); the rest are richer creature types.
    /// Salvaged from PixygonMicro's <c>AnimaTypes</c>.</summary>
    public enum AnimaType {
        Physical,
        Fire, Earth, Water, Air, Aether,   // the universe elements
        Bio, Crystal, Ice, Lightning, Occult, Holy,
    }

    /// <summary>Personality that biases growth: ±10% to offensive or defensive stats.</summary>
    public enum Nature {
        Neutral,
        Aggressive,  // +10% offence
        Defensive,   // +10% defence
    }

    /// <summary>Status ailments a creature can carry.</summary>
    public enum AnimaAilment {
        None, Poison, Sleep, Curse, Confusion, Silence, Bind,
    }

    /// <summary>Where an owned anima currently lives — the permadeath stake.</summary>
    public enum AnimaLocation {
        /// <summary>Safe, in the account ItemBox; portable across all games.</summary>
        AccountStorage,
        /// <summary>Checked into a game — ACTIVE and AT RISK under that game's rules.</summary>
        Deployed,
    }
}
