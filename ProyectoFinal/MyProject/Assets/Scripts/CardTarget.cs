using UnityEngine;

namespace Combat
{
    public class CardTarget : MonoBehaviour
    {
        [SerializeField] Player player; 

        Fighter enemy;

        private void Awake()
        {
            enemy = this.gameObject.GetComponent<Fighter>();
        }

        public void PointerEnter()
        {
            if (player.GetChosenCard() != null &&
                player.GetChosenCard().GetCard().getCardType() == Card.CardType.Attack)
            {
                player.SetTarget(enemy);
            }

        }

        public void PointerExit()
        {
            player.SetTarget(null);
        }
    }
}
