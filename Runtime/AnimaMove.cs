using Pixygon.ID;
using UnityEngine;

namespace Pixygon.Anima {
    public enum HitType { SingleEnemy, AllEnemies, AllAllies, Self, RandomEnemy }

    /// <summary>An anima move/attack (salvaged from PixygonMicro's <c>AnimaMove</c>).</summary>
    [CreateAssetMenu(menuName = "Pixygon/Anima/Anima Move", fileName = "Move")]
    public class AnimaMove : IdObject {
        public string _displayName;
        [TextArea] public string _description;

        public int _baseDamage;
        public int _apCost;
        [Range(0f, 1f)] public float _critChance = 0.1f;
        [Range(0f, 1f)] public float _accuracy = 1f;

        public AnimaType _type;
        public HitType _hitType = HitType.SingleEnemy;
        public int _numberOfAttacks = 1;

        public AnimaAilment _inflicts = AnimaAilment.None;
        [Range(0f, 1f)] public float _inflictChance;
    }
}
