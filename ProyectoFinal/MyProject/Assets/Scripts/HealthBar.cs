using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Combat
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image blockBackground;
        [SerializeField] Image blockIcon;

        [SerializeField] TMP_Text blockNumberText;
        [SerializeField] TMP_Text healthText;

        [SerializeField] Slider healthSlider;

        public Slider GetHealthBar() { return healthSlider; }

        public void DisplayBlock(int blockAmount)
        {
            if (blockAmount > 0)
            {
                blockBackground.enabled = true;
                blockIcon.enabled = true;

                blockNumberText.text = blockAmount.ToString();
                blockNumberText.enabled = true;
            }

            else
            {
                blockBackground.enabled = false;
                blockIcon.enabled = false;
                blockNumberText.enabled = false;
            }
        }
        public void DisplayHealth(int healthAmount)
        {
            healthText.text = $"{healthAmount}/{healthSlider.maxValue}";
            healthSlider.value = healthAmount;
        }
    }
}

