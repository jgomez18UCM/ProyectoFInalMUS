using UnityEngine.UI;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] HealthBar healthBar;

        [SerializeField] Image vulnerableIcon;
        [SerializeField] Image strongIcon;
        [SerializeField] Image weakIcon;

        Buff vulnerable;
        Buff weak;
        Buff strong;

        int currentHealth;
        int currentBlock;

        private void Start()
        {
            currentHealth = maxHealth;
            currentBlock = 0;

            healthBar.SetHealthValue(currentHealth); 

            healthBar.DisplayHealth(currentHealth);

            vulnerableIcon.sprite = vulnerable.icon;
            strongIcon.sprite = strong.icon;
            weakIcon.sprite = weak.icon;

            vulnerableIcon.enabled = false;
            strongIcon.enabled = false;
            weakIcon.enabled = false; 
        }

        public void TakeDamage(int amount)
        {
            if (currentBlock > 0)
                amount = BlockDamage(amount);

            currentHealth -= amount;
            UpdateHealthUI(currentHealth);

            if (currentHealth <= 0)
                Destroy(gameObject);
        }

        public void UpdateHealthUI(int newAmount)
        {
            currentHealth = newAmount;

            healthBar.DisplayHealth(newAmount);
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
            {
                vulnerable.value += amount;

                if (!vulnerableIcon.enabled)
                    vulnerableIcon.enabled = true; 
            }

            else if (type == Buff.Type.weak)
            {
                weak.value += amount;

                if (!weakIcon.enabled)
                    weakIcon.enabled = true;
            }

            else if (type == Buff.Type.strong)
            {
                strong.value += amount;

                if (!strongIcon.enabled)
                    strongIcon.enabled = true;
            }
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
            {
                vulnerable.value -= 1;

                if (vulnerable.value == 0)
                    vulnerableIcon.enabled = false;
            }

            else if (weak.value > 0)
            {
                weak.value -= 1;

                if (weak.value == 0)
                    weakIcon.enabled = false;
            }
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