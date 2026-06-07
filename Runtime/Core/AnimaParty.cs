using System.Collections.Generic;

namespace Pixygon.Anima {
    /// <summary>
    /// The account-owned vault seam. A game implements this with the Passport ItemBox
    /// (`PixygonApi.DepositItems`/`WithdrawItems`) so anima stays decoupled from the account package.
    /// Depositing returns an anima to safe storage; withdrawing deploys it into the game (at risk).
    /// </summary>
    public interface IAnimaVault {
        IReadOnlyList<AnimaInstance> Stored { get; }
        void Deposit(AnimaInstance anima);   // → AccountStorage (safe, portable)
        void Withdraw(AnimaInstance anima);  // → Deployed (into this game, at risk)
    }

    /// <summary>
    /// The in-game roster of <b>deployed</b> animas, and the <b>permadeath rule</b>: any anima still
    /// deployed when the run/lineage ends is <b>lost</b>. ("Your animas are yours — but die with them
    /// in the game, without extracting them, and they're gone.") Engine-portable.
    /// </summary>
    public sealed class AnimaParty {
        public const int MaxSize = 6;

        private readonly List<AnimaInstance> _members = new();
        public IReadOnlyList<AnimaInstance> Members => _members;
        public bool IsFull => _members.Count >= MaxSize;

        public bool Add(AnimaInstance a) {
            if (a == null || IsFull || _members.Contains(a)) return false;
            a.Location = AnimaLocation.Deployed;   // now at risk
            _members.Add(a);
            return true;
        }

        public void Remove(AnimaInstance a) => _members.Remove(a);

        /// <summary>
        /// ⚠️ <b>Permadeath.</b> Call when the run/lineage ends without the player extracting their
        /// animas (e.g. a Veilwalkers heir's death). Returns the lost animas so the game/vault can drop
        /// them from the account — they are <b>gone</b>.
        /// </summary>
        public List<AnimaInstance> ForfeitDeployed() {
            var lost = new List<AnimaInstance>(_members);
            _members.Clear();
            return lost;
        }
    }
}
