using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;


namespace Combat
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] Image intentIcon;
        [SerializeField] Text intentAmount;

        [SerializeField] List<EnemyAction> enemyActions;

        [SerializeField] Fighter player;

        [SerializeField] GameObject self; 

        List<EnemyAction> turns;
        
        int turnNumber;

        Fighter enemy;

        private void Start()
        {
            turnNumber = 0;

            self.SetActive(true); 

            turns = new List<EnemyAction>();

            enemy = this.gameObject.GetComponent<Fighter>();

            GenerateTurns();
        }


        private void LoadEnemy()
        {
            GenerateTurns();
        }


        public Fighter GetFigtherEnemy() { return enemy; }


        public void DissapearBody() { self.SetActive(false); }


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

            turns.Shuffle();
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

            //player.AddBuff(t, turns[turnNumber].debuffAmount);
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
