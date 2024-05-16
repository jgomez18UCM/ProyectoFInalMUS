using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CardActions : MonoBehaviour
    {
        [SerializeField] Fighter player;

        Fighter target;
        Card card; 

        BattleSceneManager battleSceneManager;

        private void Awake()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
        }
        public void PerformAction(Card c, Fighter t)
        {
            card = c;
            target = t;

            switch (card.GetCardName())
            {
                case "Strike":
                    AttackEnemy();
                    break;
                case "Defend":
                    PerformBlock();
                    break;
                case "Bash":
                    AttackEnemy();
                    ApplyBuff(Buff.Type.vulnerable);
                    break;
                case "Inflame":
                    ApplyBuffToSelf(Buff.Type.strong);
                    break;
                case "Clothesline":
                    AttackEnemy();
                    ApplyBuff(Buff.Type.weak);
                    break;
                case "IronWave":
                    AttackEnemy();
                    PerformBlock();
                    break;
                case "Bloodletting":
                    AttackSelf();
                    battleSceneManager.energy += 2;
                    break;
                case "Bodyslam":
                    BodySlam();
                    break;
                case "Entrench":
                    Entrench();
                    break;
                default:
                    Debug.Log("Carta no existente");
                    break;
            }
        }
        private void AttackEnemy()
        {
            int damage = card.GetCardEffectAmount() + player.getWeak().value;

            VulnerableAttack(damage);

            target.TakeDamage(damage);
        }

        private void AttackStrength()
        {
            int damage = card.GetCardEffectAmount() + (player.getStrength().value * 3);

            VulnerableAttack(damage); 
            
            target.TakeDamage(damage);
        }

        private int VulnerableAttack(int damage)
        {
            if (target.getVulnerable().value > 0)
                damage *= 2;

            return damage; 
        }

        private void BodySlam()
        {
            int damage = player.getBlock();

            VulnerableAttack(damage); 

            target.TakeDamage(damage);
        }

        private void Entrench()
        {
            player.AddBlock(player.getBlock());
        }

        private void ApplyBuff(Buff.Type t)
        {
            target.AddBuff(t, card.GetBuffAmount());
        }

        private void ApplyBuffToSelf(Buff.Type t)
        {
            player.AddBuff(t, card.GetBuffAmount());
        }

        private void AttackSelf()
        {
            player.TakeDamage(2);
        }

        private void PerformBlock()
        {
            player.AddBlock(card.GetCardEffectAmount());
        }
    }
}
