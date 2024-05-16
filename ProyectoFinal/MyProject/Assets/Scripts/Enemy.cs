using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Combat
{
    public struct EnemyAction
    {
        public IntentType intentType;
        public enum IntentType { Attack, Block, StrategicBuff, StrategicDebuff, AttackDebuff }

        public int chance;
        public int amount;
        public int debuffAmount;

        public Buff.Type buffType;

        public Sprite icon;
    }
    public class Enemy : MonoBehaviour
    {
        public List<EnemyAction> enemyActions;
        public List<EnemyAction> turns = new List<EnemyAction>();
        public int turnNumber;
        public bool shuffleActions;
        public Fighter thisEnemy;

        [Header("UI")]
        public Image intentIcon;
        public TMP_Text intentAmount;

        [Header("Specifics")]
        BattleSceneManager battleSceneManager;
        Fighter player;
        Animator animator;
        public bool midTurn;

        private void Start()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            player = battleSceneManager.player;
            thisEnemy = GetComponent<Fighter>();
            animator = GetComponent<Animator>();

            if (shuffleActions)
                GenerateTurns();
        }
        private void LoadEnemy()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            player = battleSceneManager.player;
            thisEnemy = GetComponent<Fighter>();

            if (shuffleActions)
                GenerateTurns();
        }
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
                case EnemyAction.IntentType.StrategicBuff:
                    ApplyBuffToSelf(turns[turnNumber].buffType);
                    StartCoroutine(ApplyBuff());
                    break;
                case EnemyAction.IntentType.StrategicDebuff:
                    ApplyDebuffToPlayer(turns[turnNumber].buffType);
                    StartCoroutine(ApplyBuff());
                    break;
                case EnemyAction.IntentType.AttackDebuff:
                    ApplyDebuffToPlayer(turns[turnNumber].buffType);
                    StartCoroutine(AttackPlayer());
                    break;
                default:
                    Debug.Log("El enemigo no ha hecho ninguna accion");
                    break;
            }
        }
        public void GenerateTurns()
        {
            foreach (EnemyAction eA in enemyActions)
            {
                for (int i = 0; i < eA.chance; i++)
                    turns.Add(eA);
            }
            //turns.Shuffle();
        }

        private IEnumerator AttackPlayer()
        {
            animator.Play("Attack");

            int totalDamage = turns[turnNumber].amount + thisEnemy.getStrength().value;

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

            thisEnemy.EvaluateBuffsAtTurnEnd();
            midTurn = false;
        }
        private void ApplyBuffToSelf(Buff.Type t)
        {
            thisEnemy.AddBuff(t, turns[turnNumber].amount);
        }
        private void ApplyDebuffToPlayer(Buff.Type t)
        {
            if (player == null)
                LoadEnemy();

            player.AddBuff(t, turns[turnNumber].debuffAmount);
        }
        private void PerformBlock()
        {
            thisEnemy.AddBlock(turns[turnNumber].amount);
        }
        public void DisplayIntent()
        {
            if (turns.Count == 0)
                LoadEnemy();

            intentIcon.sprite = turns[turnNumber].icon;

            if (turns[turnNumber].intentType == EnemyAction.IntentType.Attack)
            {
                int totalDamage = turns[turnNumber].amount + thisEnemy.getStrength().value;

                if (player.getVulnerable().value > 0)
                    totalDamage *= 2;

                intentAmount.text = totalDamage.ToString();
            }

            else
                intentAmount.text = turns[turnNumber].amount.ToString();
        }
    }
}
