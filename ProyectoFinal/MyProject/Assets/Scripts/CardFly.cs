using UnityEngine;

namespace Combat
{
    public class CardFly : MonoBehaviour
    {
        public Transform targetPosition;

        [SerializeField] Player player; 

        private void Awake()
        {
            player = GetComponent<Player>(); 
            targetPosition = player.GetDiscardText().transform;
        }

        public void Update()
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition.position, Time.deltaTime * 10);

            if (Vector3.Distance(this.transform.position, targetPosition.position) < 1f)
                Destroy(this.gameObject);
        }
    }
}
