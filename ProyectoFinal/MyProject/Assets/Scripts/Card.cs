using UnityEngine.UI; 
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu]
    public class Card : ScriptableObject
    {
        [SerializeField] Sprite cardIcon;
        [SerializeField] CardInfo card;


        CardClass cardClass;
        CardTarget cardTarget;
        CardType cardType;
        

        public enum CardClass { Normal, Curse}
        public enum CardTarget { Self, Enemy };
        public enum CardType { Attack, Defense, Buff }
        

        public int GetCardCost() { return card.cost; }
        public int GetCardEffectAmount() { return card.value; }
        public int GetBuffAmount() { return card.buff.value; }


        public Buff.Type getBuffType() { return card.buff.type; }


        public CardType getCardType() { return cardType; }
        public CardClass getCardClass() { return cardClass; }
        public CardTarget getCardTarget() { return cardTarget; }


        public void SetCardType(CardType type) { cardType = type; }
        public void SetCardClass(CardClass clas) { cardClass = clas; }
        public void SetCardTarget(CardTarget target) { cardTarget = target; }


        public string GetCardDescription() { return card.description; }
        public string GetCardName() { return card.name; }


        public void SetCardIcon(Sprite icon) { cardIcon = icon;}
        public Sprite GetCardIcon() { return cardIcon; }
    }


    [System.Serializable]
    public struct CardInfo
    {
        public string description; 
        public string name; 

        public int value;
        public int cost;

        public Buff buff;
    }


    [System.Serializable]
    public struct Buff
    {
        public enum Type { none, strong, vulnerable, weak }
        public Type type;

        public Sprite icon;

        public int value;
    }
}
