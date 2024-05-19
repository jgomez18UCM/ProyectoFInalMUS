using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Combat
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] Color winColor;
        [SerializeField] Color loseColor;
        // Start is called before the first frame update
        void Start()
        {
            if (text != null)
                text.text = "";
        }

        public void SetState(bool win)
        {
            if (text != null)
            {
                if (win)
                {
                    text.text = "You Win!";
                    text.color = winColor;
                }
                else
                {
                    text.text = "You Lose!";
                    text.color = loseColor;
                }
            }
        }
    }
}

