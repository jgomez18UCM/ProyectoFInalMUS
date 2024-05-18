using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Combat
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Text energyText;
        [SerializeField] Text discardText;
        [SerializeField] Text drawText;

        [SerializeField] List<Card> deck;
        [SerializeField] List<CardUI> cardsUIInHand;

        List<Card> drawPile;
        List<Card> cardsInHand;
        List<Card> discardPile;

        [SerializeField] int maxEnergy;

        [SerializeField] Fighter fighter;
        
        Fighter target;

        CardUI chosenCard;
        CardUI auxiliarCard;

        CardActions cardActions; 

        int drawAmount;
        int energy;

        private void Start()
        {
            cardActions = this.gameObject.GetComponent<CardActions>(); 

            chosenCard = new CardUI();
            auxiliarCard = new CardUI();

            target = new Fighter(); 

            drawAmount = 5;

            discardPile = new List<Card>();
            cardsInHand = new List<Card>();
            drawPile = new List<Card>();

            discardPile.AddRange(deck); 

            ShuffleCards(); 

            DrawCards(drawAmount);
        }

        public void ShuffleCards()
        {
            discardPile.Shuffle();

            drawPile = discardPile;

            discardPile = new List<Card>();

            discardText.text = discardPile.Count.ToString();
        }

        public void DrawCards(int amountToDraw)
        {
            int cardsDrawn = 0;

            energy = maxEnergy;
            energyText.text = energy.ToString();

            while (cardsDrawn < amountToDraw && cardsInHand.Count <= 10)
            {
                if (drawPile.Count < 1)
                    ShuffleCards();

                cardsInHand.Add(drawPile[0]);

                DisplayCardInHand(drawPile[0]);

                drawPile.Remove(drawPile[0]);

                drawText.text = drawPile.Count.ToString();

                cardsDrawn++;
            }
        }

        public void DisplayCardInHand(Card card)
        {
            auxiliarCard = cardsUIInHand[cardsInHand.Count - 1];

            auxiliarCard.LoadCard(card);

            auxiliarCard.gameObject.SetActive(true);
        }

        public void SetTarget(Fighter t) { target = t; }

        public void PlayCard(CardUI cardUI)
        {
            cardActions.PerformAction(cardUI.GetCard(), target);

            energy -= cardUI.GetCard().GetCardCost();
            energyText.text = energy.ToString();

            cardUI.gameObject.SetActive(false);
            cardsInHand.Remove(cardUI.GetCard());
            DiscardCard(cardUI.GetCard());
        }


        public void DiscardCard(Card card)
        {
            discardPile.Add(card);
            discardText.text = discardPile.Count.ToString();
        }

        public void DiscardHand()
        {
            foreach (Card card in cardsInHand)
            {
                //cardsInHand.Remove(card);
                //DiscardCard(card); 
            }
        }


        public void SetEnergy(int value) { energy = value; }
        public int GetEnergy() { return energy; }


        public Fighter GetFigther() { return fighter; }


        public int GetDrawAmount() { return drawAmount; }


        public Text GetDiscardText() { return discardText; }


        public void SetChosenCard(CardUI c) { chosenCard = c; }
        public CardUI GetChosenCard() { return chosenCard; }
    }
}
