using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBeat : MonoBehaviour
{
    [SerializeField] private RectTransform buttonTransform;
    [SerializeField] float pulseSize = 1.15f;
    [SerializeField] float returnSpeed = 5f;
    Vector3 startSize;

    // Start is called before the first frame update
    void Start()
    {
        startSize = buttonTransform.localScale;
        //StartCoroutine(beating());

    }

    // Update is called once per frame
    void Update()
    {
        buttonTransform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);
    }

    public void pulse() { buttonTransform.localScale = startSize * pulseSize; }

  
}
