using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using FMODUnity;

public class PressButton : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] GameObject instrument; 


    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (instrument != null)
            {
                if (instrument.activeInHierarchy) instrument.SetActive(false);
                else instrument.SetActive(true); 
            }

            Debug.Log(key.ToString());
        }
    }
}
