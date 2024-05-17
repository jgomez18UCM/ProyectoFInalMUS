using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace Combat
{
    public class SceneManager : MonoBehaviour
    {
        public Fighter cardTarget;
        public Fighter p;

        [SerializeField] Player player;

        int maxEnergy;

        enum Turn { Player, Enemy };
        Turn turn;

        [SerializeField] Button endTurnButton;

        [SerializeField] List<Enemy> enemies;

        [SerializeField] GameObject[] possibleEnemies;

        [SerializeField] Text turnText;

        public void StartFight()
        {
            List<Enemy> enemies = new List<Enemy>();

            BeginBattle(possibleEnemies);
        }

        public void BeginBattle(GameObject[] prefabsArray)
        {
            turnText.text = "Player's Turn";

            Enemy[] enemiesArray = FindObjectsOfType<Enemy>();

            enemies = new List<Enemy>();

            foreach (Enemy e in enemiesArray)
            {
                enemies.Add(e);
                e.DisplayIntent();
            }

            maxEnergy = player.GetEnergy();
        }

        public void ChangeTurn()
        {
            if (turn == Turn.Player)
            {
                turn = Turn.Enemy;
                endTurnButton.enabled = false;

                foreach (Enemy e in enemies)
                {
                    e.GetFigtherEnemy().setBlock(0);

                    e.GetFigtherEnemy().getHealthBar().DisplayBlock(0);
                }

                p.EvaluateBuffsAtTurnEnd();
                StartCoroutine(HandleEnemyTurn());
            }

            else
            {
                foreach (Enemy e in enemies)
                    e.DisplayIntent();

                turn = Turn.Player;

                p.setBlock(0);

                p.getHealthBar().DisplayBlock(0);

                player.SetEnergy(maxEnergy);

                endTurnButton.enabled = true;

                player.DrawCards(player.GetDrawAmount());

                turnText.text = "Player's Turn";
            }
        }
        private IEnumerator HandleEnemyTurn()
        {
            turnText.text = "Enemy's Turn";

            yield return new WaitForSeconds(1.5f);

            foreach (Enemy enemy in enemies)
                enemy.TakeTurn();

            ChangeTurn();
        }

        public void EndFight(bool win)
        {
            p.ResetBuffs();
        }
    }
}
