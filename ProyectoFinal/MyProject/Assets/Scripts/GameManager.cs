using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header ("Volume Parameters")]
    [SerializeField]
    float volume;
    public float getVolume() { return volume; }

    //[SerializeField]
    //StudioEventEmitter soundEffect;

    float puntuacionFinal = 0;
    public float getPuntuacionFinal() { return puntuacionFinal; }
    public void setPuntuacionFinal(float p) { puntuacionFinal = p; }
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        RuntimeManager.StudioSystem.setParameterByName("Volumen", volume);
    }

    public void changeVolume(float vol) 
    { 
        volume = vol;
        RuntimeManager.StudioSystem.setParameterByName("Volumen", volume);
    }

    public void exit()
    {
        Application.Quit();
    }
}
