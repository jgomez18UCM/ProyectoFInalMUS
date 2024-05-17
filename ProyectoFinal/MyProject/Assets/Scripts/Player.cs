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

        CardUI chosenCard; 

        int drawAmount;
        int energy;

        private void Start()
        {
            drawAmount = 5;

            cardsUIInHand = new List<CardUI>();
            discardPile = new List<Card>();
            cardsInHand = new List<Card>();
            drawPile = new List<Card>();

            Begin(); 
        }

        public void Begin()
        {
            ShuffleCards();
            DrawCards(drawAmount);
        }

        public void ShuffleCards()
        {
            drawPile = discardPile;

            discardText.text = discardPile.Count.ToString();
        }

        public void DrawCards(int amountToDraw)
        {
            int cardsDrawn = 0;

            energy = maxEnergy;
            energyText.text = energy.ToString();

            while (cardsDrawn < amountToDraw && cardsInHand.Count <= 10)
            {
                Debug.Log(cardsInHand.Count); 

                if (drawPile.Count < 1)
                    ShuffleCards();

                cardsInHand.Add(drawPile[cardsDrawn]);

                DisplayCardInHand(drawPile[cardsDrawn]);

                drawPile.Remove(drawPile[cardsDrawn]);

                drawText.text = drawPile.Count.ToString();

                cardsDrawn++;
            }
        }

        public void DisplayCardInHand(Card card)
        {
            CardUI cardUI = cardsUIInHand[cardsInHand.Count - 1];
            cardUI.LoadCard(card);
            cardUI.gameObject.SetActive(true);
        }

        public void PlayCard(CardUI cardUI)
        {
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


        public void SetEnergy(int value) { energy = value; }
        public int GetEnergy() { return energy; }


        public Fighter GetFigther() { return fighter; }


        public int GetDrawAmount() { return drawAmount; }


        public Text GetDiscardText() { return discardText; }


        public void SetChosenCard(CardUI c) { chosenCard = c; }
        public CardUI GetChosenCard() { return chosenCard; }
    }
}
