using UnityEngine;

namespace Combat
{
    [CreateAssetMenu]
    public class EnemyAction : ScriptableObject
    {
        public IntentType intentType;
        public enum IntentType { Attack, Block, Buff, Debuff }

        public int chance;
        public int amount;
        public int debuffAmount;

        public Buff.Type buffType;

        public Sprite icon;
    }
}

