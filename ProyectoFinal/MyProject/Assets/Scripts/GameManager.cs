using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.UI;

namespace Combat 
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Volume Parameters")]
        [SerializeField]
        float volume;
        public float getVolume() { return volume; }

        public List<Card> playerDeck = new List<Card>();
        public List<Card> cardLibrary = new List<Card>();

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

            //playerStatsUI = FindObjectOfType<PlayerStatsUI>();
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

        public void DisplayHealth(int healthAmount, int maxHealth)
        {
            //playerStatsUI.healthDisplayText.text = $"{healthAmount} / {maxHealth}";
        }
    }
}

