using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Combat
{
public class BuffUI : MonoBehaviour
{
	Image buffImage;
    TMP_Text buffText;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void DisplayBuff(Buff b)
    {
        animator.Play("IntentSpawn");
        buffImage.sprite = b.icon;
        buffText.text = b.value.ToString();
    }
}
}
