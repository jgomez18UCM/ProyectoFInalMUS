using UnityEngine.UI;
using UnityEngine;

namespace Combat
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image blockBackground;
        [SerializeField] Image blockIcon;

        [SerializeField] Text blockText;
        [SerializeField] Text healthText;

        Slider healthSlider;

        private void Start()
        {
            healthSlider = this.gameObject.GetComponent<Slider>();

            ActiveBlockElements(false);
        }


        public Slider GetHealthBar() { return healthSlider; }

        public void SetHealthValue(int health) 
        { 
            healthSlider.maxValue = health;

            DisplayHealth(health); 
        }


        public void DisplayBlock(int blockAmount)
        {
            if (blockAmount > 0)
            {
                ActiveBlockElements(true);
                blockText.text = blockAmount.ToString();
            }

            else
                ActiveBlockElements(false); 
        }

        private void ActiveBlockElements(bool block)
        {
            blockBackground.enabled = block;
            blockText.enabled = block;
            blockIcon.enabled = block;
        }

        public void DisplayHealth(int healthAmount)
        {
            healthText.text = $"{healthAmount}/{healthSlider.maxValue}";

            healthSlider.value = healthAmount;
        }
    }
}

