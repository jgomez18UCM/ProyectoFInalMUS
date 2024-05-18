using UnityEngine;

namespace Combat
{
    public class CardActions : MonoBehaviour
    {
        Player player;

        Fighter fighter;
        Fighter target;

        Card card; 

        private void Awake()
        {
            player = this.gameObject.GetComponent<Player>();

            fighter = player.GetFigther(); 
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
                case "Block":
                    PerformBlock();
                    break;
                case "Bash":
                    AttackEnemy();
                    ApplyBuff(Buff.Type.vulnerable);
                    break;
                case "Thunderclap":
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
                    player.SetEnergy(player.GetEnergy() + 2);
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
            int damage = card.GetCardEffectAmount() + fighter.getWeak().value;

            VulnerableAttack(damage);

            if (target != null)
                target.TakeDamage(damage);
        }

        private void AttackStrength()
        {
            int damage = card.GetCardEffectAmount() + (fighter.getStrength().value * 3);

            VulnerableAttack(damage);

            if (target != null)
                target.TakeDamage(damage);
        }

        private int VulnerableAttack(int damage)
        {
            if (target != null && target.getVulnerable().value > 0)
                damage *= 2;

            return damage; 
        }

        private void BodySlam()
        {
            int damage = fighter.getBlock();

            VulnerableAttack(damage);

            if (target != null)
                target.TakeDamage(damage);
        }

        private void Entrench()
        {
            fighter.AddBlock(fighter.getBlock());
        }

        private void ApplyBuff(Buff.Type t)
        {
            if (target != null)
                target.AddBuff(t, card.GetBuffAmount());
        }

        private void ApplyBuffToSelf(Buff.Type t)
        {
            fighter.AddBuff(t, card.GetBuffAmount());
        }

        private void PerformBlock()
        {
            fighter.AddBlock(card.GetCardEffectAmount());
        }
    }
}
