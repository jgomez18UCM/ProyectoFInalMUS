using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Combat
{
public class BattleSceneManager : MonoBehaviour
{
    [Header("Cards")]
    public List<Card> deck;
    public List<Card> drawPile = new List<Card>();
    public List<Card> cardsInHand = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public CardUI selectedCard;
    public List<CardUI> cardsInHandGameObjects = new List<CardUI>();

    [Header("Stats")]
    public Fighter cardTarget;
    public Fighter player;
    public int maxEnergy;
    public int energy;
    public int drawAmount = 5;
    
    enum Turn {Player, Enemy};
    Turn turn;

    [Header("UI")]
    public Button endTurnButton; 
    public TMP_Text drawPileCountText;
    public TMP_Text discardPileCountText;
    public TMP_Text energyText;
    public Transform topParent;
    public Transform enemyParent;
    
    [Header("Enemies")]
    public List<Enemy> enemies = new List<Enemy>();
    List<Fighter> enemyFighters = new List<Fighter>();

    public GameObject[] possibleEnemies;

    CardActions cardActions;
    GameManager gameManager;

    public TMP_Text turnText;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cardActions = GetComponent<CardActions>();
    }
    public void StartFight()
    {
        BeginBattle(possibleEnemies);
    }

	public void BeginBattle(GameObject[] prefabsArray)
    {
        turnText.text = "Player's Turn";

        GameObject newEnemy = Instantiate(prefabsArray[Random.Range(0,prefabsArray.Length)], enemyParent);

        Enemy[] enemiesArray = FindObjectsOfType<Enemy>();
        enemies = new List<Enemy>();

        discardPile = new List<Card>();
        drawPile = new List<Card>();
        cardsInHand = new List<Card>();

        foreach(Enemy e in enemiesArray)
        {
                enemies.Add(e);
                enemyFighters.Add(e.GetComponent<Fighter>());
        }

        foreach (Enemy e in enemies) 
            e.DisplayIntent();
        
        discardPile.AddRange(gameManager.playerDeck);
        ShuffleCards();
        DrawCards(drawAmount);
        energy = maxEnergy;
        energyText.text=energy.ToString();
    }

    public void ShuffleCards()
    {
        //discardPile.Shuffle();
        drawPile = discardPile;
        discardPile = new List<Card>();
        discardPileCountText.text = discardPile.Count.ToString();
    }
    public void DrawCards(int amountToDraw)
	{
        int cardsDrawn = 0;
		while(cardsDrawn<amountToDraw&&cardsInHand.Count<=10)
        {
            if(drawPile.Count<1)
                ShuffleCards();

            cardsInHand.Add(drawPile[0]);
            DisplayCardInHand(drawPile[0]);
            drawPile.Remove(drawPile[0]);
            drawPileCountText.text = drawPile.Count.ToString();
            cardsDrawn++;
        }
	}
    public void DisplayCardInHand(Card card)
    {
        CardUI cardUI = cardsInHandGameObjects[cardsInHand.Count-1];
        cardUI.LoadCard(card);
        cardUI.gameObject.SetActive(true);
    }

    public void PlayCard(CardUI cardUI)
    {
        cardActions.PerformAction(cardUI.card, cardTarget);

        energy -= cardUI.card.GetCardCost();
        energyText.text = energy.ToString();

        Instantiate(cardUI.discardEffect, cardUI.transform.position, Quaternion.identity, topParent);
        selectedCard = null;
        cardUI.gameObject.SetActive(false);
        cardsInHand.Remove(cardUI.card);
        DiscardCard(cardUI.card);
    }

    public void DiscardCard(Card card)
    {
        discardPile.Add(card);
        discardPileCountText.text = discardPile.Count.ToString();
    }

    public void ChangeTurn()
    {
        if (turn == Turn.Player)
        {
            turn = Turn.Enemy;
            endTurnButton.enabled=false;
            
            foreach (Enemy e in enemies)
            {
                if (e.thisEnemy == null)
                    e.thisEnemy = e.GetComponent<Fighter>();

                e.thisEnemy.setBlock(0);

                e.thisEnemy.getHealthBar().DisplayBlock(0);
            }

            player.EvaluateBuffsAtTurnEnd();
            StartCoroutine(HandleEnemyTurn());
        }

        else
        {
            foreach(Enemy e in enemies)
                    e.DisplayIntent();
            
            turn = Turn.Player;

            player.setBlock(0);

            player.getHealthBar().DisplayBlock(0);

            energy = maxEnergy;
            energyText.text = energy.ToString();

            endTurnButton.enabled = true;
            DrawCards(drawAmount);

            turnText.text = "Player's Turn";
        }
    }
    private IEnumerator HandleEnemyTurn()
    {
        turnText.text = "Enemy's Turn";

        yield return new WaitForSeconds(1.5f);

        foreach(Enemy enemy in enemies)
        {
            enemy.midTurn = true;
            enemy.TakeTurn();

            while(enemy.midTurn)
                yield return new WaitForEndOfFrame();
        }

        ChangeTurn();
    }

    public void EndFight(bool win)
    {
        player.ResetBuffs();
    }
}
}
