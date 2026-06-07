# Pixygon — Anima

Non-humanoid creatures — this universe's manifested fantasy life. **Capturable**,
**party-joinable** (Pokémon / SMT-style), and — the platform's signature — **account-
owned and extractable**: *your animas are yours, and travel between games.* Creatures =
`Actor` + `Anima`.

> **The permadeath stake:** an anima still *deployed in a game* when you die is **lost**.
> Extract it to your account (the ItemBox) to keep it safe. "Pokémon as it should have
> been."

## Why it's shaped this way

- **Engine-portable core.** `Pixygon.Anima.Core` (`noEngineReferences`) holds the whole
  data model + rules — pure C#, lifts onto the future engine and serializes into the
  account ItemBox / any save.
- **Thin Unity adapter.** `Pixygon.Anima` adds the authoring assets + the creature body.
- **Salvaged** from PixygonMicro's near-complete Anima system (stats formula, nature,
  EVs, bestiary, moves, types) — cleaned up (bestiary is injectable, not a static
  singleton; default-creature ids moved out of code).

```
com.pixygon.anima/
└── Runtime/
    ├── Core/   →  Pixygon.Anima.Core   ⚙️ engine-free
    │   ├── AnimaEnums.cs     AnimaType / Nature / AnimaAilment / AnimaLocation
    │   ├── AnimaStatLine.cs  the 7 creature stats + growth formula
    │   ├── AnimaInstance.cs  an OWNED creature (effective stats, ownership/permadeath state)
    │   ├── AnimaBestiary.cs  the Pokédex (seen/defeated/captured) — injectable
    │   └── AnimaParty.cs     IAnimaVault seam + AnimaParty (ForfeitDeployed = permadeath)
    └── (root) →  Pixygon.Anima          Unity adapter
        ├── AnimaDefinition.cs : IdObject — a species (base stats, type, art, move pool)
        ├── AnimaMove.cs       : IdObject — a move
        ├── AnimaCatalog.cs    the game's species set
        └── AnimaBody.cs       MonoBehaviour + IBody (the creature's body)
```

## Key concepts

- **`AnimaDefinition`** (species) → **`AnimaInstance`** (your owned creature).
  `EffectiveStats = floor(level · (base/50 + EV) · nature)`.
- **`AnimaLocation`** — `AccountStorage` (safe, portable) vs `Deployed` (in a game, at
  risk). **`AnimaParty.ForfeitDeployed()`** is the permadeath hook (call on
  death-without-extraction, e.g. a Veilwalkers heir's death).
- **`IAnimaVault`** — the account seam; a game implements it with the Passport ItemBox
  (`Deposit`/`Withdraw`/`Mint`), keeping anima decoupled from the account package.

## Stat mapping (→ `com.pixygon.stats`)

When an anima drives an Actor's `StatBlock`: Vitality→Health, PhysicalAttack→
PhysicalPower, SpecialAttack→MagicPower, PhysicalDefence→Defense, SpecialDefence→
MagicDefense, Speed→Agility. Poise (break-stun) stays anima-internal.

## Dependencies

`com.pixygon.idsystem`, `com.pixygon.actors`.

## Usage

```csharp
var inst = catalog.Get(speciesId).MakeInstance(level: 5, Nature.Aggressive);
party.Add(inst);                 // deploy (now at risk)
vault.Deposit(inst);             // …or extract to the account (safe)
var lost = party.ForfeitDeployed();   // on death without extraction → gone
```

## Status

`0.1.0` — MVP scaffold (data model + capture/party/bestiary + permadeath + body seam).
**Next:** battle/move resolution, type-effectiveness table, evolution chains, the
`StatBlock` projection, and the real ItemBox `IAnimaVault` impl (Passport). Veilwalkers'
existing Anima SOs migrate onto `AnimaDefinition`. `.meta` files generate on first
Unity import.
