using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Combat
{
    public class EnemyAudio : MonoBehaviour
    {
        StudioEventEmitter emitter;
        // Start is called before the first frame update
        void Start()
        {
            emitter = GetComponent<StudioEventEmitter>();
            if (emitter == null)
            {
                Debug.LogError("An object with an EnemyAudio should have an StudioEventEmmitter Component");
                this.enabled = false;
                return;
            }
           
        }

        public void SetDebuffed(bool isDebuffed)
        {
            if (emitter == null) return;
            if (isDebuffed)
            {
                emitter.SetParameter("Debuff", 1);
            }
            else
            {
                emitter.SetParameter("Debuff", 0);
            }
        }

        public void SetAlive(bool isAlive)
        {
            if (emitter == null) return;
            if (isAlive)
            {
                emitter.SetParameter("Alive", 1);
            }
            else
            {
                emitter.SetParameter("Alive", 0);
            }
        }
    }
}