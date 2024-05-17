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
        [SerializeField] List<Card> drawPile;
        [SerializeField] List<Card> cardsInHand;
        [SerializeField] List<Card> discardPile;

        [SerializeField] int maxEnergy;

        [SerializeField] Fighter figther;  

        List<CardUI> cardsInHandGameObjects;

        CardUI chosenCard; 

        int drawAmount;
        int energy;

        private void Start()
        {
            drawAmount = 5;
            energy = maxEnergy;

            cardsInHandGameObjects = new List<CardUI>();
            discardPile = new List<Card>();
            cardsInHand = new List<Card>();
            drawPile = new List<Card>();

            discardPile.AddRange(deck);
            drawPile.AddRange(deck); 
        }

        public void Begin(GameObject[] prefabsArray)
        {
            ShuffleCards();
            DrawCards(drawAmount);

            energyText.text = energy.ToString();
        }

        public void ShuffleCards()
        {
            drawPile = discardPile;

            discardText.text = discardPile.Count.ToString();
        }

        public void DrawCards(int amountToDraw)
        {
            int cardsDrawn = 0;

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
            CardUI cardUI = cardsInHandGameObjects[cardsInHand.Count - 1];
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


        public Fighter GetFigther() { return figther; }


        public int GetDrawAmount() { return drawAmount; }


        public Text GetDiscardText() { return discardText; }


        public void SetChosenCard(CardUI c) { chosenCard = c; }
        public CardUI GetChosenCard() { return chosenCard; }
    }
}
