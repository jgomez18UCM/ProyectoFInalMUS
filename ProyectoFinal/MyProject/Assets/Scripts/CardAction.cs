using UnityEngine;

namespace Combat
{
    public class CardActions : MonoBehaviour
    {
        [SerializeField] Player player;

        Fighter playerFighter;
        Fighter target;
        Card card; 

        private void Awake()
        {
            player = GetComponent<Player>();
            playerFighter = player.GetFigther(); 
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
            int damage = card.GetCardEffectAmount() + playerFighter.getWeak().value;

            VulnerableAttack(damage);

            target.TakeDamage(damage);
        }

        private void AttackStrength()
        {
            int damage = card.GetCardEffectAmount() + (playerFighter.getStrength().value * 3);

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
            int damage = playerFighter.getBlock();

            VulnerableAttack(damage); 

            target.TakeDamage(damage);
        }

        private void Entrench()
        {
            playerFighter.AddBlock(playerFighter.getBlock());
        }

        private void ApplyBuff(Buff.Type t)
        {
            target.AddBuff(t, card.GetBuffAmount());
        }

        private void ApplyBuffToSelf(Buff.Type t)
        {
            playerFighter.AddBuff(t, card.GetBuffAmount());
        }

        private void PerformBlock()
        {
            playerFighter.AddBlock(card.GetCardEffectAmount());
        }
    }
}
