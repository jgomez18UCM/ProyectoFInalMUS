using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private Material idle;
    private MeshRenderer mesh;

    private bool pressed; 

    [SerializeField] Material material;

    [SerializeField] KeyCode key; 

    void Start()
    {
        mesh = GetComponent<MeshRenderer>(); 

        idle = mesh.material;

        pressed = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            mesh.material = material;
            pressed = true; 
        }

        else if (Input.GetKeyUp(key))
        {
            mesh.material = idle;
            pressed = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pressed)
            Destroy(other.gameObject);
    }
}
