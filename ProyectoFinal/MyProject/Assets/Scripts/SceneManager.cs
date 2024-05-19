using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

using static UnityEngine.EventSystems.EventTrigger;

namespace Combat
{
    public class SceneManager : MonoBehaviour
    {
        enum Turn { Player, Enemy };
        Turn turn;

        [SerializeField] Player player;

        [SerializeField] Button endTurnButton;

        [SerializeField] GameObject[] enemies;

        [SerializeField] Text turnText;

        List<Enemy> enemiesArray;

        private void Start()
        {
            enemiesArray = new List<Enemy>();

            BeginBattle();
        }

        public void BeginBattle()
        {
            //turnText.text = "Player's Turn";

            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy e = enemies[i].GetComponent<Enemy>();

                enemiesArray.Add(e);

                e.DisplayIntent(); 
            }
        }

        public void ChangeTurn()
        {
            if (turn == Turn.Player)
            {
                player.DiscardHand();

                turn = Turn.Enemy;
                endTurnButton.enabled = false;

                int aliveEnemies = 0;
                for (int i = 0; i < enemiesArray.Count; i++)
                {
                    
                    if (enemiesArray[i].gameObject.activeInHierarchy)
                    {
                        Enemy e = enemiesArray[i];

                        e.GetFigtherEnemy().setBlock(0);

                        e.GetFigtherEnemy().getHealthBar().DisplayBlock(0);
                        aliveEnemies++;
                    }
                    
                }
                if (aliveEnemies == 0)
                    GameManager.EndGame(true);

                player.GetFigther().EvaluateBuffsAtTurnEnd();
                StartCoroutine(HandleEnemyTurn());
            }

            else
            {
                for (int i = 0; i < enemiesArray.Count; i++)
                {
                    if (enemiesArray[i].gameObject.activeInHierarchy)
                    {
                        Enemy e = enemiesArray[i];

                        e.DisplayIntent();
                    }
                }

                turn = Turn.Player;

                player.GetFigther().setBlock(0);

                player.GetFigther().getHealthBar().DisplayBlock(0);

                endTurnButton.enabled = true;

                player.DrawCards(player.GetDrawAmount());

                //turnText.text = "Player's Turn";
            }
        }

        private IEnumerator HandleEnemyTurn()
        {
            //turnText.text = "Enemy's Turn";

            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < enemiesArray.Count; i++)
            {
                if (enemiesArray[i].gameObject.activeInHierarchy)
                {
                    Enemy e = enemiesArray[i].GetComponent<Enemy>();

                    e.TakeTurn();
                }
            }

            ChangeTurn();
        }

        public void EndFight()
        {
            player.GetFigther().ResetBuffs();
        }

    }
}
