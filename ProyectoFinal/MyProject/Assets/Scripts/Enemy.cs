using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;


namespace Combat
{
    public struct EnemyAction
    {
        public IntentType intentType;
        public enum IntentType { Attack, Block, Buff, Debuff }

        public int chance;
        public int amount;
        public int debuffAmount;

        public Buff.Type buffType;

        public Sprite icon;
    }


    public class Enemy : MonoBehaviour
    {
        [SerializeField] Image intentIcon;
        [SerializeField] Text intentAmount;

        List<EnemyAction> enemyActions;
        List<EnemyAction> turns;
        
        int turnNumber;
        
        bool shuffle;

        Fighter player;
        Fighter enemy;

        private void Start()
        {
            turns = new List<EnemyAction>();

            enemy = GetComponent<Fighter>();

            if (shuffle)
                GenerateTurns();
        }


        private void LoadEnemy()
        {
            if (shuffle)
                GenerateTurns();
        }


        public Fighter GetFigtherEnemy() { return enemy; }


        public void TakeTurn()
        {
            switch (turns[turnNumber].intentType)
            {
                case EnemyAction.IntentType.Attack:
                    StartCoroutine(AttackPlayer());
                    break;
                case EnemyAction.IntentType.Block:
                    PerformBlock();
                    StartCoroutine(ApplyBuff());
                    break;
                case EnemyAction.IntentType.Buff:
                    ApplyBuffToSelf(turns[turnNumber].buffType);
                    StartCoroutine(ApplyBuff());
                    break;
                case EnemyAction.IntentType.Debuff:
                    ApplyDebuffToPlayer(turns[turnNumber].buffType);
                    StartCoroutine(ApplyBuff());
                    break;
                default:
                    Debug.Log("El enemigo no ha hecho ninguna accion");
                    break;
            }
        }
        public void GenerateTurns()
        {
            foreach (EnemyAction enemiesArray in enemyActions)
            {
                for (int i = 0; i < enemiesArray.chance; i++)
                    turns.Add(enemiesArray);
            }

            //turns.Shuffle();
        }

        private IEnumerator AttackPlayer()
        {
            int totalDamage = turns[turnNumber].amount + enemy.getStrength().value;

            if (player.getVulnerable().value > 0)
                totalDamage *= 2;

            yield return new WaitForSeconds(0.5f);
            player.TakeDamage(totalDamage);

            yield return new WaitForSeconds(0.5f);
            WrapUpTurn();
        }


        private IEnumerator ApplyBuff()
        {
            yield return new WaitForSeconds(1f);
            WrapUpTurn();
        }


        private void WrapUpTurn()
        {
            turnNumber++;

            if (turnNumber == turns.Count)
                turnNumber = 0;

            enemy.EvaluateBuffsAtTurnEnd();
        }


        private void ApplyBuffToSelf(Buff.Type t)
        {
            enemy.AddBuff(t, turns[turnNumber].amount);
        }


        private void ApplyDebuffToPlayer(Buff.Type t)
        {
            if (player == null)
                LoadEnemy();

            player.AddBuff(t, turns[turnNumber].debuffAmount);
        }


        private void PerformBlock()
        {
            enemy.AddBlock(turns[turnNumber].amount);
        }


        public void DisplayIntent()
        {
            if (turns.Count == 0)
                LoadEnemy();

            intentIcon.sprite = turns[turnNumber].icon;

            if (turns[turnNumber].intentType == EnemyAction.IntentType.Attack)
            {
                int totalDamage = turns[turnNumber].amount + enemy.getStrength().value;

                if (player.getVulnerable().value > 0)
                    totalDamage *= 2;

                intentAmount.text = totalDamage.ToString();
            }

            else
                intentAmount.text = turns[turnNumber].amount.ToString();
        }
    }
}
