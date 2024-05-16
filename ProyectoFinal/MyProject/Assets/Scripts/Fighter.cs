using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour
    {
        
        [SerializeField] int maxHealth;
        [SerializeField] HealthBar healthBar;
        [SerializeField] List<Card> cards; 

        Buff vulnerable;
        Buff weak;
        Buff strong;

        int currentHealth;
        int currentBlock;

        public GameObject buffPrefab;
        public Transform buffParent;

        [SerializeField] bool isPlayer;

        Enemy enemy;

        BattleSceneManager battleSceneManager;

        public GameObject damageIndicator;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            battleSceneManager = FindObjectOfType<BattleSceneManager>();

            currentHealth = maxHealth;
            currentBlock = 0; 

            healthBar.GetHealthBar().maxValue = maxHealth;

            healthBar.DisplayHealth(currentHealth);

            //if (isPlayer)
                //gameManager.DisplayHealth(currentHealth, currentHealth);
        }

        public void TakeDamage(int amount)
        {
            if (currentBlock > 0)
                amount = BlockDamage(amount);

            currentHealth -= amount;
            UpdateHealthUI(currentHealth);

            if (currentHealth <= 0)
            {
                if (enemy != null)
                    battleSceneManager.EndFight(true);
                else
                    battleSceneManager.EndFight(false);

                Destroy(gameObject);
            }
        }

        public void UpdateHealthUI(int newAmount)
        {
            currentHealth = newAmount;

            healthBar.DisplayHealth(newAmount);

            //if (isPlayer)
                //gameManager.DisplayHealth(newAmount, maxHealth);
        }

        public void AddBlock(int amount)
        {
            currentBlock += amount;

            healthBar.DisplayBlock(currentBlock);
        }

        private void Die()
        {
            this.gameObject.SetActive(false);
        }

        private int BlockDamage(int amount)
        {
            if (currentBlock >= amount)
            {
                currentBlock -= amount;
                amount = 0;
            }

            else
            {
                amount -= currentBlock;
                currentBlock = 0;
            }

            healthBar.DisplayBlock(currentBlock);

            return amount;
        }

        public void AddBuff(Buff.Type type, int amount)
        {
            if (type == Buff.Type.vulnerable)
                vulnerable.value += amount;

            else if (type == Buff.Type.weak)
                weak.value += amount;

            else if (type == Buff.Type.strong)
                strong.value += amount;
        }

        public Buff getWeak() { return weak; }
        public Buff getStrength() { return strong; }
        public Buff getVulnerable() { return vulnerable; }

        public int getHealthPoints() { return currentHealth; }

        public int getBlock() { return currentBlock; }
        public void setBlock(int block) { currentBlock = block; }

        public HealthBar getHealthBar() { return healthBar; }

        public void EvaluateBuffsAtTurnEnd()
        {
            if (vulnerable.value > 0)
                vulnerable.value -= 1;

            else if (weak.value > 0)
                weak.value -= 1;
        }
        public void ResetBuffs()
        {
            if (vulnerable.value > 0)
                vulnerable.value = 0;

            else if (weak.value > 0)
                weak.value = 0;

            else if (strong.value > 0)
                strong.value = 0;

            currentBlock = 0;

            healthBar.DisplayBlock(0);
        }
    }
}