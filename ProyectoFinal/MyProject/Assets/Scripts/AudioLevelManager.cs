using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UIElements;


namespace Combat { 
public class AudioLevelManager : MonoBehaviour
{
    public static AudioLevelManager instance;

    [SerializeField]
    int nBeatDifficulty;

    [SerializeField]
    int tempo;

    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// Todas las siguientes variables son relativos a FMOD
    /// </summary>
    // Variables para recoger datos de las canciones
    class TrackInfo
    {
        public bool muted;
        public FMOD.Studio.EventInstance instrument; 
    }

    TrackInfo[] tracksInfo;

    FMOD.Studio.EventInstance up;
    FMOD.Studio.EventInstance down;
    FMOD.Studio.EventInstance right;
    FMOD.Studio.EventInstance left;

    [SerializeField]
    StudioEventEmitter upInstr;
    [SerializeField]
    StudioEventEmitter downIntr;
    [SerializeField]
    StudioEventEmitter rightInstr;
    [SerializeField]
    StudioEventEmitter leftInstr;

    [SerializeField] GameObject[] blocks;

    float volumen; 

    void OnGUI()
    {
        //if (tracksInfo != null && tracksInfo.Length > 1)
            //GUILayout.Box(String.Format("Current Bar = {0}, Last Marker = {1}", tracksInfo[0].CurrentMusicBar, (string)tracksInfo[1].LastMarker));
    }

    void Start()
    {
        // Creamos un callback y le asignamos el efecto que queremos que tenga
        //beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
        volumen = GameManager.instance.getVolume();

        // Creamos una instancia a partir de la referencia del evento
        up = new FMOD.Studio.EventInstance();
        right = new FMOD.Studio.EventInstance();
        left = new FMOD.Studio.EventInstance();
        down = new FMOD.Studio.EventInstance();

        tracksInfo = new TrackInfo[4];

        for (int i = 0; i < blocks.Length; i++)
        {
            tracksInfo[i] = new TrackInfo();
            tracksInfo[i].muted = false;

            switch(i)
            {
                case 0:
                    up = upInstr.EventInstance; 
                    tracksInfo[i].instrument = up;
                    break;
                case 1:
                    down = downIntr.EventInstance;
                    tracksInfo[i].instrument = down;
                    break;
                case 2:
                    right = rightInstr.EventInstance;
                    tracksInfo[i].instrument = right;
                    break;
                case 3:
                    left = leftInstr.EventInstance;
                    tracksInfo[i].instrument = left;
                    break;
                default:
                    break; 
            }

            tracksInfo[i].instrument.setVolume(volumen);
            tracksInfo[i].instrument.start();
        }

     }

    void Update()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != null)
            {
                    if (!blocks[i].activeInHierarchy)
                    {
                        if (!tracksInfo[i].muted)
                        {
                            tracksInfo[i].muted = true;
                            tracksInfo[i].instrument.setVolume(0);
                            Debug.Log(blocks[i] + " muteado");
                        }


                    }

                    else
                    {
                        if (tracksInfo[i].muted)
                        {
                            tracksInfo[i].muted = false;
                            Debug.Log(blocks[i] + " desmuteado");
                            tracksInfo[i].instrument.setVolume(volumen);
                        }


                    }
            }
        }
    }

    //[AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    //static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    //{
    //    // Creamos una nueva instancia a partir del puntero
    //    FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

    //    // Sacamos toda la informacion de la instancia
    //    IntPtr timelineInfoPtr;
    //    FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

    //    // Control de errores, por si no conseguimos los datos
    //    if (result != FMOD.RESULT.OK)
    //    {
    //        Debug.LogError("Timeline Callback error: " + result);
    //    }
    //    else if (timelineInfoPtr != IntPtr.Zero)
    //    {
    //        // Guardamos los datos del objeto y detalles de los marcadores
    //        GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
    //        TrackInfo timelineInfo = (TrackInfo)timelineHandle.Target;

    //        // Dependiendo del tipo de evento que pille
    //        switch (type)
    //        {
    //            case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
    //                {
    //                    var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
    //                    break;
    //                }
    //            case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
    //                {
    //                    var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
    //                    break;
    //                }
    //            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
    //                {
    //                    // Liberamos memoria al terminar
    //                    timelineHandle.Free();
    //                    break;
    //                }
    //        }
    //    }
    //    return FMOD.RESULT.OK;
    //}
}
}