using UnityEngine.UI;
using UnityEngine;

namespace Combat
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] Card card;

        [SerializeField] Text cardDescriptionText;
        [SerializeField] Text cardCostText;

        [SerializeField] Image cardImage;

        [SerializeField] Player player;

        public Card GetCard() { return card; }

        public void LoadCard(Card c)
        {
            card = c;

            cardDescriptionText.text = card.GetCardDescription();

            cardCostText.text = card.GetCardCost().ToString();

            cardImage.sprite = card.GetCardIcon();
        }

        public void SelectCard()
        {
            player.SetChosenCard(this);
        }

        public void DeselectCard()
        {
            player.SetChosenCard(null);
        }

        public void HandleEndDrag()
        {
            if (player.GetEnergy() < card.GetCardCost())
                return;

            player.PlayCard(this);
        }
    }
}
