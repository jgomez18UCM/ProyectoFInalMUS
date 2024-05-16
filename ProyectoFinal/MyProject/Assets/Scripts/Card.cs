using UnityEngine;

namespace Combat
{
    [CreateAssetMenu]
    public class Card : ScriptableObject
    {
        public Sprite cardIcon;

        CardInfo card;

        CardClass cardClass;
        CardTarget cardTarget;
        CardType cardType;
        
        public enum CardClass { Normal, Curse}
        public enum CardTarget { Self, Enemy };
        public enum CardType { Attack, Defense, Buff }
        

        public int GetCardCost() { return card.cost; }
        public int GetCardEffectAmount() { return card.value; }
        public int GetBuffAmount() { return card.buff; }

        public Buff.Type getBuffType() { return card.buffType; }

        public CardType getCardType() { return cardType; }
        public CardClass getCardClass() { return cardClass; }
        public CardTarget getCardTarget() { return cardTarget; }

        public string GetCardDescription() { return card.description; }
        public string GetCardName() { return card.name; }
    }

    [System.Serializable]
    public struct CardInfo
    {
        public string description; 
        public string name; 

        public int value;
        public int buff;
        public int cost; 

        public Buff.Type buffType;
    }

    [System.Serializable]
    public struct Buff
    {
        public enum Type { strong, vulnerable, weak }
        public Type type;

        public Sprite icon;

        public int value;
    }
}
