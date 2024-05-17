using UnityEngine;

namespace Combat
{
    public class CardTarget : MonoBehaviour
    {
        Player player; 
        Fighter enemy;

        private void Awake()
        {
            player = GetComponent<Player>(); 
            enemy = GetComponent<Fighter>();
        }

        public void PointerEnter()
        {
            if (enemy == null)
                enemy = GetComponent<Fighter>();

            if (player.GetChosenCard() != null && 
                player.GetChosenCard().GetCard().getCardType() == Card.CardType.Attack)
                player.GetChosenCard().GetCard().SetCardTarget(Card.CardTarget.Enemy);
        }
        public void PointerExit()
        {
            player.GetChosenCard().GetCard().getCardTarget(); 
        }
    }
}
