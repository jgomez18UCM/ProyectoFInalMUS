using UnityEngine.UI;
using UnityEngine;


namespace Combat
{
    public class BuffUI : MonoBehaviour
    {
        Image buffImage;
        Text buffText;

        public void DisplayBuff(Buff b)
        {
            buffImage.sprite = b.icon;
            buffText.text = b.value.ToString();
        }
    }
}
